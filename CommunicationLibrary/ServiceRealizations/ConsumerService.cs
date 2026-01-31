using AdditionalHelpers.Services;
using CommunicationLibrary.Services;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;

namespace CommunicationLibrary.ServiceRealizations;

public abstract class ConsumerService : BackgroundService, IConsumerService
{
    private readonly ConnectionFactory _factory;
    private IConnection _connection = null!;
    private readonly ConcurrentDictionary<string, IChannel> _channels = [];
    private readonly ConcurrentDictionary<string, AsyncEventingBasicConsumer> _consumers = [];
    protected readonly ISerializer _serializer;
    protected readonly ILoggerService _loggerService;

    public ConsumerService(RabbitMQSettings settings, ISerializer serializer, ILoggerService loggerService)
    {
        _factory = new ConnectionFactory
        {
            HostName = settings.HostName,
            Port = settings.Port,
            UserName = settings.UserName,
            Password = settings.Password,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };
        _serializer = serializer;
        _loggerService = loggerService;
    }

    public abstract Task Initialize();

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _connection = await _factory.CreateConnectionAsync(cancellationToken);
            _connection.ConnectionShutdownAsync += (sender, args) =>
            {
                _loggerService.Warning($"RabbitMQ connection shutdown: {args.ReplyText}");
                return Task.CompletedTask;
            };
            await Initialize();
            _loggerService.Info("Consumer service was initialized");
            await base.StartAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _loggerService.Error("Error", ex);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    protected async Task StartExchange(string exchange, string type = ExchangeType.Direct)
    {
        IChannel channel = await _connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync(exchange: exchange, type: type, autoDelete: false, durable: true);
        _channels.TryAdd(exchange, channel);
    }

    public async Task ConsumeFromQueueAsync<T>(
       string exchange,
       string queueName,
       string routingKey,
       Func<T, Task> handler)
    {
        await StartExchange(exchange);
        if (!_channels.TryGetValue(exchange, out IChannel? channel))
            throw new ApplicationException($"Channel is null {routingKey}");
        
        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        await channel.QueueBindAsync(
            queue: queueName,
            exchange: exchange,
            routingKey: routingKey);

        AsyncEventingBasicConsumer consumer = new(channel);
        consumer.ReceivedAsync += async (sender, args) =>
        {
            try
            {
                await handler(_serializer.Deserialize<T>(args.Body.ToArray())!);
                await channel.BasicAckAsync(args.DeliveryTag, multiple: false);
            }
            catch(Exception ex)
            {
                _loggerService.Error("Can't handle message", ex);
                await channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: true);
            }
        };
        _consumers.TryAdd(await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer), consumer);
    }
}

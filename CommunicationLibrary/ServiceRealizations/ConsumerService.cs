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
    private readonly IConnection _connection;
    private readonly ConcurrentDictionary<string, IChannel> _channels = [];
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
        };
        _connection = _factory.CreateConnectionAsync().Result;
        _serializer = serializer;
        _loggerService = loggerService;
    }

    public abstract Task Initialize();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Initialize();
            while (stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _loggerService.Error("Error", ex);
        }
    }

    protected async Task StartExchange(string exchange, string type = ExchangeType.Direct)
    {
        IChannel channel = await _connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync(exchange: exchange, type: type, autoDelete: false, durable: true);
        _channels.TryAdd(exchange, channel);
    }

    public async Task<string> ConsumeFromQueueAsync<T>(
       string exchange,
       string queueName,
       string routingKey,
       Func<T, Task> handler)
    {
        await StartExchange(exchange);
        var channel = await _connection.CreateChannelAsync();

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

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, args) =>
        {
            try
            {
                await handler(_serializer.Deserialize<T>(args.Body.ToArray())!);
                await channel.BasicAckAsync(args.DeliveryTag, multiple: false);
            }
            catch
            {
                await channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: true);
            }
        };

        return await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer);
    }
}

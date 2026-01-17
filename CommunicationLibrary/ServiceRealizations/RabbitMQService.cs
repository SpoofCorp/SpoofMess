using AdditionalHelpers.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;

namespace CommunicationLibrary.ServiceRealizations;

public class RabbitMQService
{
    private readonly ConnectionFactory _factory;
    private readonly ConcurrentDictionary<string, IChannel> _channels = [];
    private readonly IConnection _connection;
    protected readonly ISerializer _serializer;

    public RabbitMQService(string hostName, int port, ISerializer serializer)
    {
        _factory = new ConnectionFactory { HostName = hostName, Port = port };
        _connection = _factory.CreateConnectionAsync().Result;
        _serializer = serializer;
    }

    protected async Task Publish(string exchange, string routingKey, byte[] body, string type = ExchangeType.Direct)
    {
        if (!_channels.TryGetValue(exchange, out var channel))
            throw new KeyNotFoundException($"Exchange doesn't exists {exchange}");

        BasicProperties props = new()
        {
            Persistent = true,
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
            ContentType = "application/json"
        };

        await channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: props,
            body: body);
    }

    protected async Task StartExchange(string exchange, string type = ExchangeType.Direct)
    {
        IChannel channel = await _connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync(exchange: exchange, type: type, autoDelete: false, durable: true);
        if (!_channels.TryAdd(exchange, channel))
            throw new InvalidOperationException($"Exchange already exists {exchange}");
    }

    public async Task<string> ConsumeFromQueueAsync<T>(
       string exchange,
       string queueName,
       string routingKey,
       Func<T, Task> handler)
    {
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

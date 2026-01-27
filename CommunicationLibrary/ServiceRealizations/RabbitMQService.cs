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
    public RabbitMQService(RabbitMQSettings settings, ISerializer serializer)
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
    }

    protected async Task Publish(string exchange, string routingKey, byte[] body, string type = ExchangeType.Direct)
    {
        if (!_channels.TryGetValue(exchange, out IChannel? channel))
        {
            await StartExchange(exchange, type);
            if (!_channels.TryGetValue(exchange, out channel))
                throw new KeyNotFoundException($"Exchange doesn't exists {exchange}");
        }

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
}

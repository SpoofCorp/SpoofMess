using AdditionalHelpers.Services;
using RabbitMQ.Client;
using System.Collections.Concurrent;
using System.Text;

namespace CommunicationLibrary.ServiceRealizations;

public abstract class RabbitMQService
{
    private readonly ConnectionFactory _factory;
    private readonly ConcurrentDictionary<string, IChannel> _channels = [];
    private readonly IConnection _connection;
    protected readonly ISerializer _serializer;
    protected abstract string Exchange { get; }

    public RabbitMQService(
        RabbitMQSettings settings, 
        ISerializer serializer
        )
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

    protected async Task Publish<T>(
        string routingKey, 
        T body, 
        string type = ExchangeType.Direct
        )
    {
        byte[] bodyArray = Encoding.UTF8.GetBytes(_serializer.Serialize(body));
        await Publish(routingKey, bodyArray, type);
    }

    protected async Task Publish(
        string routingKey,
        byte[] body, 
        string type = ExchangeType.Direct
        )
    {
        if (!_channels.TryGetValue(
            Exchange, 
            out IChannel? channel
            ))
        {
            await StartExchange(type);
            if (!_channels.TryGetValue(Exchange, out channel))
                throw new KeyNotFoundException($"Exchange doesn't exists {Exchange}");
        }

        BasicProperties props = new()
        {
            Persistent = true,
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
            ContentType = "application/json"
        };

        await channel.BasicPublishAsync(
            exchange: Exchange,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: props,
            body: body);
    }

    protected async Task StartExchange(
        string type = ExchangeType.Direct
        )
    {
        IChannel channel = await _connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync(
            exchange: Exchange, 
            type: type, 
            autoDelete: false, 
            durable: true
            );

        if (!_channels.TryAdd(Exchange, channel))
            throw new InvalidOperationException($"Exchange already exists {Exchange}");
    }
}

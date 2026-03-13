using AdditionalHelpers.Services;
using RabbitMQ.Client;
using System.Collections.Concurrent;
using System.Text;

namespace CommunicationLibrary.ServiceRealizations;

public abstract class PublisherService
{
    private readonly ConnectionFactory _factory;
    private readonly RabbitMQSettings _settings;
    private readonly ConcurrentDictionary<string, IChannel> _channels = [];
    private IConnection _connection = null!;
    protected readonly ISerializer _serializer;
    protected readonly ILoggerService _loggerService;
    protected abstract string Exchange { get; }

    public PublisherService(
        RabbitMQSettings settings,
        ILoggerService loggerService,
        ISerializer serializer
        )
    {
        _settings = settings;
        _factory = new ConnectionFactory
        {
            HostName = settings.HostName,
            Port = settings.Port,
            UserName = settings.UserName,
            Password = settings.Password,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };
        _loggerService = loggerService;
        _serializer = serializer;
    }
    private async Task EnsureConnectionAsync()
    {
        if (_connection is { IsOpen: true })
            return;

        while (_connection is null || !_connection.IsOpen)
        {
            _connection = await _factory.CreateConnectionAsync();
            if (_connection is null || !_connection.IsOpen)
            {
                _loggerService.Error($"RabbitMQ wasn't started\nHostName: {_settings.HostName}\tPort: {_settings.Port}");
                await Task.Delay(_settings.RetryConnectionTime);
            }
            else
                _loggerService.Info("Publisher was started");
        }
    }

    public async Task Publish<T>(
        string routingKey,
        T body,
        string type = ExchangeType.Direct
        )
    {
        byte[] bodyArray = Encoding.UTF8.GetBytes(_serializer.Serialize(body));
        await Publish(routingKey, bodyArray, type);
    }

    public async Task Publish(
        string routingKey,
        byte[] body,
        string type = ExchangeType.Direct
        )
    {
#if DEBUG
        _loggerService.Debug($"Start publish message exchange: {Exchange} routing key: {routingKey}");
#endif
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

    public async Task StartExchange(
        string type = ExchangeType.Direct
        )
    {
        var options = new CreateChannelOptions(
            publisherConfirmationsEnabled: true,
            publisherConfirmationTrackingEnabled: true
        );
        await EnsureConnectionAsync();
        IChannel channel = await _connection.CreateChannelAsync(options);
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

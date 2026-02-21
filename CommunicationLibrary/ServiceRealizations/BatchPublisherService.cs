using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Threading.Channels;

namespace CommunicationLibrary.ServiceRealizations;

public abstract class BatchPublisherService<T> : BackgroundService, IAsyncDisposable
{
    private readonly CancellationTokenSource cts = new();
    private readonly Channel<T> channel;
    private readonly List<T> Messages = [];
    private readonly Task _readTask;
    private readonly BatchPublisherSettings _settings;
    private readonly PublisherService _publisherService;
    protected abstract string RoutingKey { get; }
    protected abstract Func<T, Task> ConfirmationFunc { get; }

    public BatchPublisherService(BatchPublisherSettings settings, PublisherService publisherService)
    {
        _settings = settings;
        BoundedChannelOptions options = new(_settings.QueueCount)
        {
            SingleReader = true,
        };
        channel = Channel.CreateBounded<T>(options);
        _readTask = Task.Run(Read);
        _publisherService = publisherService;
    }

    public async Task WriteAsync(T message) =>
        await channel.Writer.WriteAsync(message);

    protected virtual async Task Flush(List<T> messages)
    {
        await _publisherService.Batch(RoutingKey, Messages, ConfirmationFunc);
    }

    public async Task Read()
    {
        Stopwatch totalTimer = new();
        Stopwatch batchTimer = new();
        long time, elapsedTime, timer;
        try
        {
            while (await channel.Reader.WaitToReadAsync(cts.Token))
            {
                totalTimer.Restart();
                while (Messages.Count < _settings.BatchCount)
                {
                    if (channel.Reader.TryRead(out var msg))
                    {
                        Messages.Add(msg);
                        batchTimer.Restart();
                        continue;
                    }
                    elapsedTime = _settings.BatchLifeTime - totalTimer.ElapsedMilliseconds;
                    time = _settings.BatchTimeNewMessage - batchTimer.ElapsedMilliseconds;
                    timer = Math.Min(time, elapsedTime);

                    if (timer <= 0) break;
                    try
                    {
                        using CancellationTokenSource timeoutCts = new(TimeSpan.FromMilliseconds(timer));
                        await channel.Reader.WaitToReadAsync(timeoutCts.Token);
                    }
                    catch (OperationCanceledException) { break; }
                }
                if (Messages.Count > 0)
                {
                    await Flush(Messages);
                    Messages.Clear();
                }
            }

        }
        catch (OperationCanceledException)
        {
            if (Messages.Count > 0)
            {
                await Flush(Messages);
                Messages.Clear();
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        await cts.CancelAsync();

        await _readTask.ConfigureAwait(false);

        cts.Dispose();
        channel.Writer.TryComplete();

        if (Messages.Count > 0)
        {
            await Flush(Messages).ConfigureAwait(false);
            Messages.Clear();
        }
        GC.SuppressFinalize(this);
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}

using AdditionalHelpers.Services;
using CommunicationLibrary.Communication;
using CommunicationLibrary.Services;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.MessageBrokers;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers.Outboxers;

public class ChatUserOutboxer(
    ISerializer serializer,
    ILoggerService loggerService,
    IInjectionService injectionService,
    IServiceScopeFactory serviceScopeFactory
    ) : BackgroundService
{
    private readonly ILoggerService _loggerService = loggerService;
    private readonly ISerializer _serializer = serializer;
    private readonly IInjectionService _injectionService = injectionService;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    protected async Task FindRecords()
    {
        List<ChatUserOutbox> records = await _injectionService.Invoke<IChatUserOutboxRepository, List<ChatUserOutbox>>(
            async (repository) =>
                await repository.GetNotSynced(DateTime.UtcNow.AddMinutes(1))
            );

        ParallelOptions options = new()
        {
            MaxDegreeOfParallelism = 10
        };
        var ordered = records
            .GroupBy(x => new { x.ChatId, x.UserId })
            .Select(x =>
                x.OrderBy(x => x.CreatedAt)
                );
        await Parallel.ForEachAsync(ordered, options, async (records, token) =>
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();

            IChatUserPublisherService publisher = scope.ServiceProvider.GetRequiredService<IChatUserPublisherService>();
            IChatUserOutboxRepository outboxRepo = scope.ServiceProvider.GetRequiredService<IChatUserOutboxRepository>();
            await _injectionService.Invoke<IChatUserPublisherService, Task>(async (publisher) =>
            {

            });
            foreach (var outbox in records)
            {
                try
                {
                    Task processingTask = outbox.Status switch
                    {
                        OutboxStatus.Create => publisher.Create(_serializer.Deserialize<CreateChatUser>(outbox.Data)),
                        OutboxStatus.Delete => publisher.Delete(_serializer.Deserialize<DeleteChatUser>(outbox.Data)),
                        OutboxStatus.Update => publisher.Update(_serializer.Deserialize<UpdateChatUser>(outbox.Data)),
                        _ => throw new NotImplementedException($"Not supported OutboxStatus {outbox.Status}")
                    };
                    await processingTask;
                    _loggerService.Info($"Create chat-user {outbox.UserId}");
                    await Confirm(outbox, outboxRepo, true);
                }
                catch (Exception ex)
                {
                    _loggerService.Error($"Invalid data of {outbox.Id}: {outbox.Data}", ex);
                    await Confirm(outbox, outboxRepo, false);
                    break;
                }
            }
        });
    }

    protected async Task Confirm(ChatUserOutbox chatUserOutbox, IChatUserOutboxRepository repository, bool status)
    {
        try
        {
            chatUserOutbox.IsSynced = status;
            chatUserOutbox.LastTryDate = DateTime.UtcNow;
            await repository.UpdateAsync(chatUserOutbox);
        }
        catch (Exception exception)
        {
            _loggerService.Error($"Database error while save ChatuserOutbox {chatUserOutbox.Id}", exception);
        }

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await FindRecords();
            await Task.Delay(5_000, stoppingToken);
        }
    }
}
using AdditionalHelpers.Services;
using CommunicationLibrary.Services;

namespace CommunicationLibrary.ServiceRealizations;

public abstract class FileMessageConsumerService<T>(RabbitMQSettings settings, ISerializer serializer, ILoggerService loggerService) : ConsumerService(settings, serializer, loggerService), IConsumerService
{
    protected override string Exchange => "file-service";
    protected abstract string FileNomination { get; }

    protected abstract Func<T, Task> ConfirmDeletedFunc { get; }
    protected abstract Func<T, Task> ConfirmAddedFunc { get; }
    protected abstract Func<T, Task> ErrorDeletedFunc { get; }
    protected abstract Func<T, Task> ErrorAddedFunc { get; }

    protected async Task ConfirmDeleted() =>
        await ConsumeFromQueueAsync($"{FileNomination}.success", $"{FileNomination}.success.deleted", ConfirmDeletedFunc);

    protected async Task ConfirmAdded() =>
        await ConsumeFromQueueAsync($"{FileNomination}.success", $"{FileNomination}.success.added", ConfirmAddedFunc);

    protected async Task ErrorDeleted() =>
        await ConsumeFromQueueAsync($"{FileNomination}.error", $"{FileNomination}.error.deleted", ErrorDeletedFunc);

    protected async Task ErrorAdded() =>
        await ConsumeFromQueueAsync($"{FileNomination}.error", $"{FileNomination}.error.deleted", ErrorAddedFunc);

    public override async Task Initialize()
    {
        await ConfirmDeleted();
        await ErrorDeleted();
        await ConfirmAdded();
        await ErrorAdded();
    }
}

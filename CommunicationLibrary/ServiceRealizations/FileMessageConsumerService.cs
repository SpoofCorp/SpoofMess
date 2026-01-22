using AdditionalHelpers.Services;
using CommunicationLibrary.Services;

namespace CommunicationLibrary.ServiceRealizations;

public abstract class FileMessageConsumerService<T>(RabbitMQSettings settings, ISerializer serializer) : RabbitMQService(settings, serializer), IConsumerService
{
    private readonly string _fileExchange = "file-service";
    protected abstract string FileNomination { get; }

    protected async Task ConfirmDeleted(Func<T, Task> func) =>
        await ConsumeFromQueueAsync(_fileExchange, $"{FileNomination}.success", $"{FileNomination}.success.deleted", func);

    protected async Task ConfirmAdded(Func<T, Task> func) =>
        await ConsumeFromQueueAsync(_fileExchange, $"{FileNomination}.success", $"{FileNomination}.success.added", func);

    protected async Task ErrorDeleted(Func<T, Task> func) =>
        await ConsumeFromQueueAsync(_fileExchange, $"{FileNomination}.error", $"{FileNomination}.error.deleted", func);

    protected async Task ErrorAdded(Func<T, Task> func) =>
        await ConsumeFromQueueAsync(_fileExchange, $"{FileNomination}.error", $"{FileNomination}.error.deleted", func);

    public abstract Task Initialize();
}

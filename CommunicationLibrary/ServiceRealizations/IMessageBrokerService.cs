using AdditionalHelpers.Services;
using CommunicationLibrary.Communication;
using CommunicationLibrary.Services;
using System.Text;

namespace CommunicationLibrary.ServiceRealizations;

public abstract class FileMessageBroker<T>(RabbitMQSettings settings, ISerializer serializer) : RabbitMQService(settings, serializer), IFileMessageBroker
{
    private readonly string _fileExchange = "file-service";
    protected abstract string fileNomination { get; }

    public async Task Create(CreateFile createFile)
    {
        byte[] body = Encoding.UTF8.GetBytes(_serializer.Serialize(createFile));
        await Publish(_fileExchange, "file.reserve", body);
    }

    public async Task Delete(DeleteFile deleteFile)
    {
        byte[] body = Encoding.UTF8.GetBytes(_serializer.Serialize(deleteFile));
        await Publish(_fileExchange, "file.delete", body);
    }

    protected async Task ConfirmDeleted(Func<T, Task> func) =>
        await ConsumeFromQueueAsync(_fileExchange, $"{fileNomination}.success", $"{fileNomination}.success.deleted", func);

    protected async Task ConfirmAdded(Func<T, Task> func) =>
        await ConsumeFromQueueAsync(_fileExchange, $"{fileNomination}.success", $"{fileNomination}.success.added", func);

    protected async Task ErrorDeleted(Func<T, Task> func) =>
        await ConsumeFromQueueAsync(_fileExchange, $"{fileNomination}.error", $"{fileNomination}.error.deleted", func);

    protected async Task ErrorAdded(Func<T, Task> func) =>
        await ConsumeFromQueueAsync(_fileExchange, $"{fileNomination}.error", $"{fileNomination}.error.deleted", func);
}
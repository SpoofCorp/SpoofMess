using AdditionalHelpers.Services;
using CommunicationLibrary.Communication;
using CommunicationLibrary.Services;
using System.Text;

namespace CommunicationLibrary.ServiceRealizations;

public abstract class FileMessagePublisherService<T>(RabbitMQSettings settings, ISerializer serializer) : RabbitMQService(settings, serializer), IFileMessagePublisherService
{
    protected override string Exchange => "file-service";

    public async Task Create(CreateFile createFile)
    {
        byte[] body = Encoding.UTF8.GetBytes(_serializer.Serialize(createFile));
        await Publish("file.reserve", body);
    }

    public async Task Delete(DeleteFile deleteFile)
    {
        byte[] body = Encoding.UTF8.GetBytes(_serializer.Serialize(deleteFile));
        await Publish("file.delete", body);
    }
}
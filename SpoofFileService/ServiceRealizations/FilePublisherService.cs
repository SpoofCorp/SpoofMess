using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofFileService.Services;

namespace SpoofFileService.ServiceRealizations;
public class FilePublisherService(
        RabbitMQSettings settings,
        ILoggerService loggerService,
        ISerializer serializer
    ) : PublisherService(
            settings,
            loggerService,
            serializer
        ), IFilePublisherService
{
    protected override string Exchange => "file-service";

    public async Task Create(CreateFile createFile)
    {
        await Publish("file.success.created", createFile);
    }

    public async Task Delete(DeleteFile deleteFile)
    {
        await Publish("file.success.deleted", deleteFile);
    }
}
using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Services;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers.Consumers;

public class FileMetadatumConsumerService(
    RabbitMQSettings settings,
    ISerializer serializer, 
    ILoggerService loggerService,
    IServiceScopeFactory serviceScopeFactory) : ConsumerService(
        settings, 
        serializer, 
        loggerService)
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    protected override string BaseQueueName => "settings.file";

    protected override string Exchange => "file-service";

    protected async Task ConfirmDeleted()
    {
        await ConsumeFromQueueAsync<DeleteFile>(
            "success",
            "file.success.deleted",
            (obj) => _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IFileMetadatumService>().Delete(obj));
    }

    protected async Task ConfirmAdded()
    {
        await ConsumeFromQueueAsync<CreateFile>(
            "success",
            "file.success.created",
            (obj) => _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IFileMetadatumService>().Save(obj));

    }

    protected async Task ErrorDeleted()
    {
        throw new NotImplementedException();
    }

    protected async Task ErrorAdded()
    {
        throw new NotImplementedException();
    }

    public override async Task Initialize()
    {
        await ConfirmDeleted();
        await ConfirmAdded();
        /*await ErrorDeleted();
        await ErrorAdded();*/
    }
}

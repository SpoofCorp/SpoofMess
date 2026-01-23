using AdditionalHelpers.Services;
using CommonObjects.DTO;
using CommunicationLibrary;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.MessageBrokers;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers;

public class ChatAvatarFileConsumerService : FileMessageConsumerService<FileMetadata>, IChatAvatarFileConsumerService
{
    private readonly string _exchange = "chatAvatar";
    private readonly IFileMetadatumRepository _fileMetadatumRepository;
    protected override string FileNomination => "chatAvatar";

    public ChatAvatarFileConsumerService(RabbitMQSettings settings, ISerializer serializer, IFileMetadatumRepository fileMetadatumRepository) : base(settings, serializer)
    {
        _fileMetadatumRepository = fileMetadatumRepository;
        StartExchange(_exchange).Wait();
    }

    public override async Task Initialize()
    {
        await ConfirmAdded(async (fileMetadata) =>
        {
            await ChangeStatus(fileMetadata.Id, OperationsStatus.Success, false);
        });
        await ConfirmDeleted(async (fileMetadata) =>
        {
            await ChangeStatus(fileMetadata.Id, OperationsStatus.Success, true);
        });
        await ErrorAdded(async (fileMetadata) =>
        {
            await ChangeStatus(fileMetadata.Id, OperationsStatus.Error, true);
        }); 
        await ErrorDeleted(async (fileMetadata) =>
        {
            await ChangeStatus(fileMetadata.Id, OperationsStatus.Error, false);
        });
    }

    private async Task ChangeStatus(Guid fileId, OperationsStatus status, bool isDeleted)
    {
        FileMetadatum? fileMetadatum = await _fileMetadatumRepository.GetByIdAsync(fileId);
        if (fileMetadatum is null)
            return;
        fileMetadatum.FileMetadataOperationStatuses.Add(new()
        {
            IsActual = true,
            OperationStatusId = (short)status,
            TimeSet = DateTime.UtcNow
        });
        fileMetadatum.IsDeleted = isDeleted;
        await _fileMetadatumRepository.UpdateAsync(fileMetadatum);
    }
}

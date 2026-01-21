using AdditionalHelpers.Services;
using CommonObjects.DTO;
using CommunicationLibrary;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.MessageBrokers;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers;

public class ChatAvatarFileService : FileMessageBroker<FileMetadata>, IChatAvatarFileService
{
    private readonly string _exchange = "chatAvatar";
    private readonly IFileMetadatumRepository _fileMetadatumRepository;
    protected override string fileNomination => "chatAvatar";

    public ChatAvatarFileService(RabbitMQSettings settings, ISerializer serializer, IFileMetadatumRepository fileMetadatumRepository) : base(settings, serializer)
    {
        _fileMetadatumRepository = fileMetadatumRepository;
        StartExchange(_exchange).Wait();
    }
    private async Task ChnageStatus(Guid fileId, OperationsStatus status, bool isDeleted)
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

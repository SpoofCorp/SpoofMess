using AdditionalHelpers.Services;
using CommonObjects.DTO;
using CommunicationLibrary;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.MessageBrokers;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers;

public class ChatAvatarFileConsumerService(RabbitMQSettings settings, ISerializer serializer, ILoggerService loggerService, IFileMetadatumRepository fileMetadatumRepository) : FileMessageConsumerService<FileMetadata>(settings, serializer, loggerService), IChatAvatarFileConsumerService
{
    private readonly IFileMetadatumRepository _fileMetadatumRepository = fileMetadatumRepository;

    protected override string FileNomination => "chatAvatar";

    protected override Func<FileMetadata, Task> ConfirmDeletedFunc => async (fileMetadata) => await ChangeStatus(fileMetadata.Id, OperationsStatus.Success, true);

    protected override Func<FileMetadata, Task> ConfirmAddedFunc => async (fileMetadata) => await ChangeStatus(fileMetadata.Id, OperationsStatus.Success, false);

    protected override Func<FileMetadata, Task> ErrorDeletedFunc => async (fileMetadata) => await ChangeStatus(fileMetadata.Id, OperationsStatus.Error, false);

    protected override Func<FileMetadata, Task> ErrorAddedFunc => async (fileMetadata) => await ChangeStatus(fileMetadata.Id, OperationsStatus.Error, true);

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

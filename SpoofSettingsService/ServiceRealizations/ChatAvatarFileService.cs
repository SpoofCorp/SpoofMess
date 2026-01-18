using AdditionalHelpers.Services;
using CommonObjects.DTO;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;
using System.Text;

namespace SpoofSettingsService.ServiceRealizations;

public class ChatAvatarFileService : RabbitMQService, IChatAvatarFileService
{
    private readonly string _exchange = "chatAvatar";
    private readonly IFileMetadatumRepository _fileMetadatumRepository;
    public ChatAvatarFileService(string hostName, int port, ISerializer serializer, IFileMetadatumRepository fileMetadatumRepository) : base(hostName, port, serializer)
    {
        _fileMetadatumRepository = fileMetadatumRepository;
        StartExchange(_exchange).Wait();
        ConfirmAdded().Wait();
        ConfirmDeleted().Wait();
    }

    public async Task CreateAvatar(FileMetadata fileMetadata)
    {
        byte[] body = Encoding.UTF8.GetBytes(_serializer.Serialize(fileMetadata));
        await Publish(_exchange, "chatAvatar.added", body);
    }

    public async Task DeleteAvatar(Guid fileId)
    {
        byte[] body = Encoding.UTF8.GetBytes(_serializer.Serialize(fileId));
        await Publish(_exchange, "chatAvatar.deleted", body);
    }

    private async Task ConfirmAdded()
    {
        await ConsumeFromQueueAsync<FileMetadata>(_exchange, "chatAvatar.success", "chatAvatar.success.added", async (file) =>
        {
            FileMetadatum? fileMetadatum = await _fileMetadatumRepository.GetByIdAsync(file.Id);
            if (fileMetadatum is null)
                return;
            fileMetadatum.FileMetadataOperationStatuses.Add(new()
            {
                IsActual = true,
                OperationStatusId = (short)OperationsStatus.Success,
                TimeSet = DateTime.UtcNow
            });
            await _fileMetadatumRepository.UpdateAsync(fileMetadatum);
        });
    }

    private async Task ConfirmDeleted()
    {
        await ConsumeFromQueueAsync<FileMetadata>(_exchange, "chatAvatar.success", "chatAvatar.success.deleted", async (file) =>
        {
            FileMetadatum? fileMetadatum = await _fileMetadatumRepository.GetByIdAsync(file.Id);
            if (fileMetadatum is null)
                return;
            fileMetadatum.FileMetadataOperationStatuses.Add(new()
            {
                IsActual = true,
                OperationStatusId = (short)OperationsStatus.Success,
                TimeSet = DateTime.UtcNow
            });
            fileMetadatum.IsDeleted = true;
            await _fileMetadatumRepository.UpdateAsync(fileMetadatum);
        });
    }
}

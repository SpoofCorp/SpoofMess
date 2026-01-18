using CommonObjects.DTO;

namespace SpoofSettingsService.Services;

public interface IChatAvatarFileService
{
    public Task DeleteAvatar(Guid fileId);
    public Task CreateAvatar(FileMetadata fileMetadata);
}

using CommonObjects.DTO;

namespace SpoofSettingsService.Services;

public interface IChatAvatarFileService
{
    public Task DeleteAvatar(FileMetadata fileMetadata);
    public Task CreateAvatar(FileMetadata fileMetadata);
}

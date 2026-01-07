using DataHelpers.Services;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Repositories;

public interface IUserAvatarRepository : IBaseRepository<UserAvatar, Guid>
{
    public Task<UserAvatar?> GetActualUserAvatarById(Guid userId);

    public Task<List<UserAvatar>?> GetUserAvatarsById(Guid userId);

    public Task<bool> TryDeleteAvatarByIds(Guid userId, Guid fileId);
}

using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class UserAvatarRepository(ICacheService cache, SpoofSettingsServiceContext context, IProcessQueueTasksService tasksService) : CachedSoftDeletableRepository<UserAvatar>(cache, context, tasksService), IUserAvatarRepository
{
    public async Task<UserAvatar?> GetActualUserAvatarById(Guid userId) =>
        await GetAsync(GetKey(userId), async () => await _set.FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive));

    public async Task<List<UserAvatar>?> GetUserAvatarsById(Guid userId) =>
        await _set.Where(x => x.UserId == userId && !x.IsDeleted).ToListAsync();

    public async Task<bool> TryDeleteAvatarByIds(Guid userId, Guid fileId)
    {
        UserAvatar? avatar = await _set.FirstOrDefaultAsync(x => x.UserId == userId && x.FileId == fileId);
        if (avatar is null)
            return false;

        await SoftDeleteAsync(avatar);
        return true;
    }

    private static string GetKey(Guid userId) =>
        $"{typeof(UserAvatar).Name.ToLower()}:{userId}";

    protected override string GetKey(UserAvatar entity) =>
        $"{typeof(UserAvatar).Name.ToLower()}:{entity.UserId}:{entity.FileId}";

}

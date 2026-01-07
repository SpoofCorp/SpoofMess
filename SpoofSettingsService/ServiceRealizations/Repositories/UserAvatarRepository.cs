using DataHelpers.ServiceRealizations;
using DataHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class UserAvatarRepository(ICacheService cache, SpoofSettingsServiceContext context, ProcessQueueTasksService tasksService) : Repository<UserAvatar, Guid>(cache, context, tasksService), IUserAvatarRepository
{
    public async Task<UserAvatar?> GetActualUserAvatarById(Guid userId) =>
        await GetAsync(GetKey(userId), async () => await _set.FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive));

    public async Task<List<UserAvatar>?> GetUserAvatarsById(Guid userId) =>
        await GetManyAsync(GetManyKey(userId), async () => await _set.Where(x => x.UserId == userId && !x.IsDeleted).ToListAsync());

    public async Task<bool> TryDeleteAvatarByIds(Guid userId, Guid fileId)
    {
        UserAvatar? avatar = await _set.FirstOrDefaultAsync(x => x.UserId == userId && x.FileId == fileId);
        if (avatar is null)
            return false;

        await SoftDeleteAsync(avatar);
        return true;
    }

    private new static string GetKey(Guid userId) =>
        $"{typeof(UserAvatar).Name.ToLower()}:{userId}";

    private static string GetManyKey(Guid userId) =>
        $"{typeof(UserAvatar).Name.ToLower()}s:{userId}";
}

using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class UserAvatarRepository(
        ICacheService cache,
        IDbContextFactory<SpoofSettingsServiceContext> factory,
        IProcessQueueTasksService tasksService
    ) : CachedSoftDeletableDoubleIdentifiedFactoryRepository<UserAvatar, Guid, Guid, SpoofSettingsServiceContext>(
        cache, 
        factory,
        tasksService
    ), IUserAvatarRepository
{
    public async Task<UserAvatar?> GetActualUserAvatarById(Guid userId)
    {
        await using SpoofSettingsServiceContext context = await _factory.CreateDbContextAsync();
        return await GetAsync(GetKey(userId), async () => 
            await context.UserAvatars.FirstOrDefaultAsync(x => 
                x.Key1 == userId 
                && x.IsActive
            )
        );
    }

    public async Task<List<UserAvatar>?> GetUserAvatarsById(Guid userId)
    {
        await using SpoofSettingsServiceContext context = await _factory.CreateDbContextAsync();
        return await context.UserAvatars.Where(x => x.Key1 == userId && !x.IsDeleted).ToListAsync();
    }

    public async Task<bool> TryDeleteAvatarByIds(Guid userId, Guid fileId)
    {
        await using SpoofSettingsServiceContext context = await _factory.CreateDbContextAsync();
        UserAvatar? avatar = await context.UserAvatars.FirstOrDefaultAsync(x =>
            x.Key1 == userId 
            && x.Key2 == fileId
        );
        if (avatar is null)
            return false;

        await SoftDeleteAsync(avatar);
        return true;
    }

    private static string GetKey(Guid userId) =>
        $"{typeof(UserAvatar).Name.ToLower()}:{userId}";

    protected override string GetKey(UserAvatar entity) =>
        $"{typeof(UserAvatar).Name.ToLower()}:{entity.Key1}:{entity.Key2}";

}

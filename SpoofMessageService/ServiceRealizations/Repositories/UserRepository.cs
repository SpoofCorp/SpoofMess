using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofMessageService.Models;
using SpoofMessageService.Services.Repositories;

namespace SpoofMessageService.ServiceRealizations.Repositories;

public class UserRepository(
    ICacheService cache,
    SpoofMessageServiceContext context,
    IProcessQueueTasksService processQueueTasks
    ) : CachedSoftDeletableIdentifiedRepository<User, Guid>(
        cache,
        context,
        processQueueTasks
        ), IUserRepository
{
    public async Task<bool> ExecuteUpdate(Guid userId, bool isConnected)
    {
        int count = await _set.Where(x => x.Equals(userId)).ExecuteUpdateAsync(x =>
            x.SetProperty(p => p.IsConnected, isConnected)
            .SetProperty(p => p.IsConnected, isConnected)
        );
        User? user = await _cache.Get<User>(GetKey(userId));
        if (user is null)
            return count > 0;
        user.IsConnected = isConnected;
        await _cache.Save(GetKey(userId), user);
        return count > 0;
    }
}

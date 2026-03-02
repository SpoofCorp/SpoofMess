using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofMessageService.Models;
using SpoofMessageService.Services.Repositories;

namespace SpoofMessageService.ServiceRealizations.Repositories;

public class UserRepository(
    ICacheService cache,
    IDbContextFactory<SpoofMessageServiceContext> factory,
    IProcessQueueTasksService processQueueTasks
    ) : CachedSoftDeletableIdentifiedFactoryRepository<User, Guid, SpoofMessageServiceContext>(
        cache,
        factory,
        processQueueTasks
        ), IUserRepository
{
    public async Task<bool> ExecuteUpdate(Guid userId, bool isConnected)
    {
        await using SpoofMessageServiceContext context = await _factory.CreateDbContextAsync();
        int count = await context.Users.Where(x => x.Id.Equals(userId)).ExecuteUpdateAsync(x =>
            x.SetProperty(p => p.IsConnected, isConnected)
        );
        User? user = await _cache.Get<User>(GetKey(userId));
        if (user is null)
            return count > 0;
        user.IsConnected = isConnected;
        await _cache.Save(GetKey(userId), user);
        return count > 0;
    }
}

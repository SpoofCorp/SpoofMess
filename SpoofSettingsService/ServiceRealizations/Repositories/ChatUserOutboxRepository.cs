using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class ChatUserOutboxRepository(
        ICacheService cache,
        IDbContextFactory<SpoofSettingsServiceContext> factory,
        IProcessQueueTasksService processQueueTasks
    ) : CachedIdentifiedFactoryRepository<ChatUserOutbox, Guid, SpoofSettingsServiceContext>(
            cache,
            factory,
            processQueueTasks
        ), IChatUserOutboxRepository
{

    public async Task<List<ChatUserOutbox>> GetNotSynced(DateTime notBefore)
    {
        await using SpoofSettingsServiceContext context = await _factory.CreateDbContextAsync();
        return await context.ChatUserOutboxes.Where(
                x => !x.IsSynced
                && x.LastTryDate < notBefore
            ).ToListAsync();
    }
}

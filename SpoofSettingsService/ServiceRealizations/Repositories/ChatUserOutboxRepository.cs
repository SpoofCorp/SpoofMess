using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class ChatUserOutboxRepository(
        ICacheService cache,
        SpoofSettingsServiceContext context,
        IProcessQueueTasksService processQueueTasks
    ) : CachedIdentifiedRepository<ChatUserOutbox, Guid>(
            cache,
            context,
            processQueueTasks
        ), IChatUserOutboxRepository
{

    public async Task<List<ChatUserOutbox>> GetNotSynced(DateTime notBefore)
    {
        return await _set.Where(
                x => !x.IsSynced
                && x.LastTryDate < notBefore
            ).ToListAsync();
    }
}

using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class ChatRoleRepository(
    ICacheService cache,
    IDbContextFactory<SpoofSettingsServiceContext> factory, 
    IProcessQueueTasksService processQueueTasks
    ) : CachedSoftDeletableIdentifiedFactoryRepository<ChatRole, long, SpoofSettingsServiceContext>(
        cache, 
        factory, 
        processQueueTasks
    ), IChatRoleRepository
{
}

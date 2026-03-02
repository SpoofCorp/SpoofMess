using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class ChatUserRuleRepository(
    ICacheService cache, 
    IDbContextFactory<SpoofSettingsServiceContext> factory, 
    IProcessQueueTasksService tasksService
    ) : CachedSoftDeletableDoubleIdentifiedFactoryRepository<ChatUserRule, Guid, Guid, SpoofSettingsServiceContext>(
        cache,
        factory,
        tasksService
    ), IChatUserRuleRepository
{

}

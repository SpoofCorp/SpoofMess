using DataHelpers.ServiceRealizations.Repositories.WithCache;
using DataHelpers.Services;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class ChatTypeRepository(ICacheService cacheService, SpoofSettingsServiceContext context, IProcessQueueTasksService queueTasksService) : CachedSoftDeletableIdentifiedRepository<ChatType, int>(cacheService, context, queueTasksService), IChatTypeRepository
{
}

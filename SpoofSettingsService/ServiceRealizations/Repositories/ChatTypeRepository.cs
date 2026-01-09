using DataHelpers.ServiceRealizations;
using DataHelpers.Services;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class ChatTypeRepository(ICacheService cacheService, SpoofSettingsServiceContext context, ProcessQueueTasksService queueTasksService) : Repository<ChatType, long>(cacheService, context, queueTasksService), IChatTypeRepository
{
}

using DataHelpers.ServiceRealizations.Repositories.WithCache;
using DataHelpers.Services;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class UserRepository(ICacheService cache, SpoofSettingsServiceContext context, IProcessQueueTasksService tasksService) : CachedSoftDeletableIdentifiedRepository<User, Guid>(cache, context, tasksService), IUserRepository
{
}

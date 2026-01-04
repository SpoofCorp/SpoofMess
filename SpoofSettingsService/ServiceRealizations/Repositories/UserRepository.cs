using DataHelpers.ServiceRealizations;
using DataHelpers.Services;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Interfaces;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class UserRepository(ICacheService cache, SpoofSettingsServiceContext context, ProcessQueueTasksService tasksService) : Repository<User, Guid>(cache, context, tasksService), IUserRepository
{
}

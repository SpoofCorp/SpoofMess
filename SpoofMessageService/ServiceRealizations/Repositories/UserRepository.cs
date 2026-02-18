using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
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
}

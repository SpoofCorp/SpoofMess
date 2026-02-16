using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using SpoofMessageService.Models;
using SpoofMessageService.Services.Repositories;

namespace SpoofMessageService.ServiceRealizations.Repositories;

public class ChatUserRepository(
    ICacheService cache,
    SpoofMessageServiceContext context,
    IProcessQueueTasksService processQueueTasks) 
    : CachedSoftDeletableDoubleIdentifiedRepository<ChatUser, Guid, Guid>(
        cache,
        context,
        processQueueTasks), IChatUserRepository
{

}

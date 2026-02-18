using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using SpoofMessageService.Models;
using SpoofMessageService.Services.Repositories;

namespace SpoofMessageService.ServiceRealizations.Repositories;

public class ChatRepository(
    ICacheService cache,
    SpoofMessageServiceContext context,
    IProcessQueueTasksService processQueueTasks)
    : CachedSoftDeletableIdentifiedRepository<Chat, Guid>(
        cache,
        context,
        processQueueTasks), IChatRepository
{
}

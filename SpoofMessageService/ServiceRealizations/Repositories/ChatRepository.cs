using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofMessageService.Models;
using SpoofMessageService.Services.Repositories;

namespace SpoofMessageService.ServiceRealizations.Repositories;

public class ChatRepository(
    ICacheService cache,
    IDbContextFactory<SpoofMessageServiceContext> factory,
    IProcessQueueTasksService processQueueTasks)
    : CachedSoftDeletableIdentifiedFactoryRepository<Chat, Guid, SpoofMessageServiceContext>(
        cache,
        factory,
        processQueueTasks), IChatRepository
{
}

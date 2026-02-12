using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofMessageService.Models;
using SpoofMessageService.Services.Repositories;

namespace SpoofMessageService.ServiceRealizations.Repositories;

public class MessageRepository(
    ICacheService cache, 
    DbContext context, 
    IProcessQueueTasksService processQueueTasks) 
    : CachedSoftDeletableIdentifiedRepository<Message, Guid>(
        cache,
        context,
        processQueueTasks), IMessageRepository
{
}

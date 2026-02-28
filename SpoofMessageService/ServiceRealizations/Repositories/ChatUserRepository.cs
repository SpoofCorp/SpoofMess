using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
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
    public async Task Delete(Guid chatId, Guid userId)
    {
        await context.ChatUsers.Where(x => x.Key1 == chatId && x.Key2 == userId).ExecuteDeleteAsync();
    }

    public async Task<List<ChatUser>> GetManyByChatId(Guid chatId)
    {
        return await _set.Where(x => x.Key1 == chatId).ToListAsync();
    }
}

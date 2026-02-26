using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofMessageService.Models;
using SpoofMessageService.Services.Repositories;

namespace SpoofMessageService.ServiceRealizations.Repositories;

public class MessageRepository(
        ICacheService cache,
        SpoofMessageServiceContext context, 
        IProcessQueueTasksService processQueueTasks
    ) 
    : CachedSoftDeletableIdentifiedRepository<Message, Guid>(
            cache,
            context,
            processQueueTasks
        ), IMessageRepository
{
    public async Task<List<Message>> GetMessagesAfterDate(
            Guid chatId,
            DateTime after,
            int take = 50
        )
    {
        return await _set.OrderByDescending(x => x.SentAt)
            .Where(m =>
                m.ChatId == chatId
                && m.SentAt >= after
            ).Take(take)
            .ToListAsync();
    }

    public async Task<List<Message>> GetMessagesBeforeDate(
            Guid chatId, 
            DateTime before,
            int take = 50
        )
    {
        return await _set.OrderByDescending(x => x.SentAt)
            .Where(m =>
                m.ChatId == chatId
                && m.SentAt <= before
            ).Take(take)
            .ToListAsync();
    }
}

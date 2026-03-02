using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofMessageService.Models;
using SpoofMessageService.Services.Repositories;

namespace SpoofMessageService.ServiceRealizations.Repositories;

public class MessageRepository(
        ICacheService cache,
        IDbContextFactory<SpoofMessageServiceContext> factory, 
        IProcessQueueTasksService processQueueTasks
    ) 
    : CachedSoftDeletableIdentifiedFactoryRepository<Message, Guid, SpoofMessageServiceContext>(
            cache,
            factory,
            processQueueTasks
        ), IMessageRepository
{
    public async Task<List<Message>> GetMessagesAfterDate(
            Guid chatId,
            DateTime after,
            int take = 50
        )
    {
        await using SpoofMessageServiceContext context = await _factory.CreateDbContextAsync();
        return await context.Messages.OrderByDescending(x => x.SentAt)
            .Include(x => x.User)
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
        await using SpoofMessageServiceContext context = await _factory.CreateDbContextAsync();
        return await context.Messages.OrderByDescending(x => x.SentAt)
            .Include(x => x.User)
            .Where(m =>
                m.ChatId == chatId
                && m.SentAt <= before
            ).Take(take)
            .ToListAsync();
    }

    public async Task<List<Message>> GetMessageSinceDate(
            Guid userId,
            DateTime after, 
            int take = 50
        )
    {
        await using SpoofMessageServiceContext context = await _factory.CreateDbContextAsync();
        return await context.Messages
            .Include(x => x.User)
            .Where(x => 
                x.UserId == userId 
                && x.SentAt >= after
            ).Take(take)
            .ToListAsync();
    }
}

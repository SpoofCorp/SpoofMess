using DataHelpers.ServiceRealizations.Repositories.WithCache;
using DataHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class ChatRepository(ICacheService cache, SpoofSettingsServiceContext context, IProcessQueueTasksService tasksService) : CachedSoftDeletableIdentifiedRepository<Chat, Guid>(cache, context, tasksService), IChatRepository
{
    public async Task<Chat?> GetByUniqueName(string name) =>
        await GetAsync(name, async () => await context.Chats.FirstOrDefaultAsync(x => x.ChatUniqueName == name));


    public async Task Change(Chat newChat, Chat? oldChat)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            if (oldChat is null)
                await AddAsync(newChat);
            else
            {
                oldChat.IsDeleted = true;
                oldChat.LastModified = DateTime.UtcNow;
                oldChat.ChatUniqueName = Guid.CreateVersion7().ToString();
                context.Chats.Update(oldChat);
                await context.AddAsync(newChat);
                await context.SaveChangesAsync();
                SaveToCache(GetKey(newChat), newChat);
                _processQueueTasks.AddTask(async () => await _cache.Delete(GetKey(oldChat)));

                await transaction.CommitAsync();
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message, ex); //for now we throws all exceptions
        }
    }
}
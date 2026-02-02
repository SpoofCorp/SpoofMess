using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class ChatUserRepository(ICacheService cache, SpoofSettingsServiceContext context, IProcessQueueTasksService tasksService) : CachedSoftDeletableDoubleIdentifiedRepository<ChatUser, Guid, Guid>(cache, context, tasksService), IChatUserRepository
{
    public async Task<ChatUser?> GetWithRules(Guid chatId, Guid userId)
    {
        return await GetAsync(
            GetKey(chatId, userId),
            async() => 
                await _set.Include(x => x.ChatUserRules).FirstOrDefaultAsync(x => x.Key1 == chatId && x.Key2 == userId)
            );
    }
}

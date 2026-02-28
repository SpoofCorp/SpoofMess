using CommonObjects.DTO;
using CommunicationLibrary.Communication;
using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class ChatUserRepository(
        ICacheService cache,
        SpoofSettingsServiceContext context,
        IProcessQueueTasksService tasksService
    ) : CachedSoftDeletableDoubleIdentifiedRepository<ChatUser, Guid, Guid>(
            cache,
            context,
            tasksService
        ), IChatUserRepository
{
    public async Task<ChatUser?> GetWithRules(Guid chatId, Guid userId)
    {
        return await GetAsync(
            GetKey(chatId, userId),
            async() => 
                await _set.
                    Include(x => x.ChatUserRules)
                    .Include(x => x.ChatUserChatRoles)
                    .FirstOrDefaultAsync(x => 
                        x.Key1 == chatId 
                        && x.Key2 == userId
                    )
            );
    }

    public async Task<List<ChatUserDTO>> GetUserChatsBeforeDate(Guid userId, DateTime before)
    {
        return await context.Database.SqlQuery<ChatUserDTO>($"SELECT c.\"Id\", c.\"ChatTypeId\", c.\"UniqueName\", c.\"Name\", get_user_permission({userId}, cu.\"ChatId\", null) AS \"Rules\" FROM \"ChatUser\" cu JOIN \"Chat\" c ON c.\"ChatId\" = cu.\"Id\" WHERE cu.\"UserId\" = {userId} AND cu.\"CreatedAt\" > {before}").ToListAsync();
    }
}
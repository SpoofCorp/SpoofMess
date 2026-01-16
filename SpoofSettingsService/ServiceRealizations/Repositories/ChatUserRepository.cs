using DataSaveHelpers.ServiceRealizations;
using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class ChatUserRepository(ICacheService cache, SpoofSettingsServiceContext context, IProcessQueueTasksService tasksService) : CachedSoftDeletableRepository<ChatUser>(cache, context, tasksService), IChatUserRepository
{
    public async Task<bool> DeleteMemberById(Guid memberId, Guid chatId)
    {
        ChatUser? member = await GetAsync(GetKey(chatId, memberId),
            async() => await context.ChatUsers.FirstOrDefaultAsync(x => x.ChatId == chatId && x.UserId == memberId));
        if (member is null)
            return false;

        await SoftDeleteAsync(member);
        return true;
    }

    protected override string GetKey(ChatUser entity) =>
        $"{entity.GetType().Name.ToLower()}:{entity.ChatId}-{entity.UserId}";

    protected virtual string GetKey(Guid chatId, Guid userId) =>
        $"{typeof(ChatUser).Name.ToLower()}:{chatId}-{userId}";
}

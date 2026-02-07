using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class ChatAvatarRepository(ICacheService cache, SpoofSettingsServiceContext context, IProcessQueueTasksService tasksService) : CachedSoftDeletableDoubleIdentifiedRepository<ChatAvatar, Guid, Guid>(cache, context, tasksService), IChatAvatarRepository
{
    public async Task<ChatAvatar?> GetActualChatAvatarById(Guid chatId) =>
        await GetAsync(GetKey(chatId), async () => await _set.FirstOrDefaultAsync(x => x.Key1 == chatId && x.IsActive));

    public async Task<List<ChatAvatar>?> GetChatAvatarsById(Guid chatId) =>
        await _set.Where(x => x.Key1 == chatId && !x.IsDeleted).ToListAsync();

    public async Task<bool> TryDeleteAvatarByIds(Guid chatId, Guid fileId)
    {
        ChatAvatar? avatar = await _set.FirstOrDefaultAsync(x => x.Key1 == chatId && x.Key2 == fileId);
        if (avatar is null)
            return false;

        await SoftDeleteAsync(avatar);
        return true;
    }

    private static string GetKey(Guid chatId) =>
        $"{typeof(UserAvatar).Name.ToLower()}:{chatId}";
}

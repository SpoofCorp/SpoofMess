using DataHelpers.Services;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Repositories;

public interface IChatAvatarRepository : IBaseRepository<ChatAvatar, Guid>
{
    public Task<ChatAvatar?> GetActualChatAvatarById(Guid chatId);

    public Task<List<ChatAvatar>?> GetChatAvatarsById(Guid chatId);

    public Task<bool> TryDeleteAvatarByIds(Guid chatId, Guid fileId);
}

using DataSaveHelpers.Services.Repositories;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Repositories;

public interface IChatAvatarRepository : IDoubleIdentifiedRepository<ChatAvatar, Guid, byte[]>
{
    public Task<ChatAvatar?> GetActualChatAvatarById(Guid chatId);

    public Task<List<ChatAvatar>?> GetChatAvatarsById(Guid chatId);

    public Task<bool> TryDeleteAvatarByIds(Guid chatId, byte[] fileId);
}

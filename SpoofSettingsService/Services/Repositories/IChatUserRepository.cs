using CommonObjects.DTO;
using DataSaveHelpers.Services.Repositories;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Repositories;

public interface IChatUserRepository : ISoftDeletableDoubleIdentifiedRepository<ChatUser, Guid, Guid>
{
    public Task<List<ChatMemberDTO>> GetPart(Guid chatId, string? lastLogin, DateTime? lastDate, int count = 10);
    public Task<ChatUser?> GetWithRules(Guid chatId, Guid userId);
    public Task<List<ChatUserDTO>> GetUserChatsAfterDate(Guid userId, DateTime after);
}

using DataHelpers.Services;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Interfaces;

public interface IChatUserRepository : IBaseRepository<ChatUser, Guid>
{
    public Task<bool> DeleteMemberById(Guid memberId, Guid chatId);
}

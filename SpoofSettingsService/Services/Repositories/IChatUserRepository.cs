using DataHelpers.Services.Repositories;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Repositories;

public interface IChatUserRepository : ISoftDeletableRepository<ChatUser>
{
    public Task<bool> DeleteMemberById(Guid memberId, Guid chatId);
}

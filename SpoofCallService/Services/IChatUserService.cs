using CommonObjects.Results;
using SpoofCallService.Models;

namespace SpoofCallService.Services;

public interface IChatUserService
{
    public Task<Result<IEnumerable<ChatUser>>> GetUserChats(Guid id);
}

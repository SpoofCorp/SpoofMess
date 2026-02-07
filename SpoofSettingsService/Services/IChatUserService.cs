using CommonObjects.Requests.ChatUsers;
using CommonObjects.Requests.Members;
using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services;

[Obsolete("Not check user permissions")]
public interface IChatUserService
{
    public Task<Result> Add(AddMemberRequest request, Guid userId);
    public Task<Result> Remove(DeleteMemberRequest request, Guid userId);
    public Task<Result<ChatUser>> Get(GetChatUserRequest request);
}

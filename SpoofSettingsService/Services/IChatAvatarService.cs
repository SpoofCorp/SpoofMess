using CommonObjects.Requests;
using CommonObjects.Responses;
using CommonObjects.Results;

namespace SpoofSettingsService.Services;

public interface IChatAvatarService
{
    public Task<Result> SetAvatar(SetChatAvatarRequest request);

    public Task<Result<ChatAvatarResponse>> GetAvatar(GetChatAvatarRequest request);

    public Task<Result<List<ChatAvatarResponse>>> GetAvatars(GetChatAvatarRequest request);

    public Task<Result> RemoveAvatar(RemoveChatAvatarRequest request);
}

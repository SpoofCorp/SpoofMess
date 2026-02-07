using CommonObjects.Requests.Avatars;
using CommonObjects.Responses;
using CommonObjects.Results;

namespace SpoofSettingsService.Services;

[Obsolete("Not check user permissions")]
public interface IChatAvatarService
{
    public Task<Result> SetAvatar(SetChatAvatarRequest request);

    public Task<Result<AvatarResponse>> GetAvatar(GetChatAvatarRequest request);

    public Task<Result<List<AvatarResponse>>> GetAvatars(GetChatAvatarRequest request);

    public Task<Result> RemoveAvatar(RemoveChatAvatarRequest request);
}

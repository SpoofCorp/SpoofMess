using CommonObjects.Requests.Avatars;
using CommonObjects.Responses;
using CommonObjects.Results;

namespace SpoofSettingsService.Services;

public interface IChatAvatarService
{
    public Task<Result> SetAvatar(SetChatAvatarRequest request);

    public Task<Result<AvatarResponse>> GetAvatar(GetChatAvatarRequest request);

    public Task<Result<List<AvatarResponse>>> GetAvatars(GetChatAvatarRequest request);

    public Task<Result> RemoveAvatar(RemoveChatAvatarRequest request);
}

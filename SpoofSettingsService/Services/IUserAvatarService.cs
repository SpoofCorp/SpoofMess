using CommonObjects.Requests.Avatars;
using CommonObjects.Responses;
using CommonObjects.Results;

namespace SpoofSettingsService.Services;

public interface IUserAvatarService
{

    public Task<Result> SetAvatar(SesUserAvatarRequest request);

    public Task<Result<AvatarResponse>> GetAvatar(GetUserAvatarRequest request);

    public Task<Result<List<AvatarResponse>>> GetAvatars(GetUserAvatarRequest request);

    public Task<Result> RemoveAvatar(RemoveUserAvatarRequest request);
}

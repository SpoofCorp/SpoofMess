using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Validators;

public interface IChatAvatarValidator
{
    public Result IsAvailable(ChatAvatar? chatAvatar);

    public Result FileIsActive(ChatAvatar? chatAvatar);

    public Result AvatarsIsAvailable(List<ChatAvatar>? chatAvatars);
}

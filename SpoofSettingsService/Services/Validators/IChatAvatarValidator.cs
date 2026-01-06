using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Validators;

public interface IChatAvatarValidator
{
    public Result IsNullOrDeleted(ChatAvatar? chatAvatar);

    public Result FileIsNullOrDeleted(ChatAvatar? chatAvatar);

    public Result AvatarsIsNullOrEmpty(List<ChatAvatar>? chatAvatars);
}

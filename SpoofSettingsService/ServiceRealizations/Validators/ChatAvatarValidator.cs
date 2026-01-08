using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class ChatAvatarValidator : IChatAvatarValidator
{
    public Result AvatarsIsAvailable(List<ChatAvatar>? chatAvatars)
    {

        if (chatAvatars is null)
            return Result.NotFoundResult("Invalid user id");

        if (chatAvatars.Count == 0)
            return Result.BadRequest("Not such avatars");

        return Result.OkResult();
    }

    public Result FileIsActive(ChatAvatar? chatAvatar)
    {
        if (chatAvatar is null)
            return Result.NotFoundResult("Invalid id");
        if (chatAvatar.IsDeleted)
            return Result.BadRequest("Avatar has been deleted");
        if (chatAvatar.FileId is null)
            return Result.BadRequest("Avatar has broken");

        return Result.SuccessResult();
    }

    public Result IsAvailable(ChatAvatar? chatAvatar)
    {
        if (chatAvatar is null)
            return Result.NotFoundResult("Invalid id");
        if (chatAvatar.IsDeleted)
            return Result.BadRequest("Avatar has been deleted");

        return Result.SuccessResult();
    }
}

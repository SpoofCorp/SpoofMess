using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class UserAvatarValidator : SoftDeletableValidator<UserAvatar>, IUserAvatarValidator
{
    public Result FileIsActive(UserAvatar? userAvatar)
    {
        Result result = IsAvailable(userAvatar);
        if (!result.Success)
            return result;
        if (userAvatar!.FileId is null)
            return Result.BadRequest("Avatar has broken");

        return Result.SuccessResult();
    }
}
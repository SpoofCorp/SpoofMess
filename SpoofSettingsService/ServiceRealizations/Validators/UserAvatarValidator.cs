using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Validators;
using DataSaveHelpers.ServiceRealizations;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class UserAvatarValidator : SoftDeletableValidator<UserAvatar>, IUserAvatarValidator
{
    public Result FileIsActive(UserAvatar? userAvatar)
    {
        Result result = IsAvailable(userAvatar);
        if (!result.Success)
            return result;

        return Result.SuccessResult();
    }
}
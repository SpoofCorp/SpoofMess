using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class ChatAvatarValidator : SoftDeletableValidator<ChatAvatar>, IChatAvatarValidator
{
    public Result FileIsActive(ChatAvatar? chatAvatar)
    {
        Result result = IsAvailable(chatAvatar);
        if (!result.Success)
            return result;

        return Result.SuccessResult();
    }
}

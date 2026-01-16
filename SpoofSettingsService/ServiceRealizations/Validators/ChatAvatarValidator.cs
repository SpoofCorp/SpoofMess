using CommonObjects.Results;
using DataSaveHelpers.ServiceRealizations;
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
        if (chatAvatar!.File is null)
            return Result.BadRequest("File was broken");

        return Result.SuccessResult();
    }
}

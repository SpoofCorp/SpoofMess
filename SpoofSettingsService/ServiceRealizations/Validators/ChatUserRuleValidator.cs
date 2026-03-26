using CommonObjects.Results;
using DataSaveHelpers.ServiceRealizations;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class ChatUserRuleValidator : SoftDeletableValidator<ChatUserRule>, IChatUserRuleValidator
{
    public Result IsAvailableAndPermitted(ChatUserRule? chatUserRule)
    {
        Result result = IsAvailable(chatUserRule);
        if (result.Success)
            result = chatUserRule!.IsPermission ? Result.OkResult() : Result.BadRequest("You're forbidden");
        return result;
    }
}

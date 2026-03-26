using CommonObjects.Results;
using DataSaveHelpers.Services;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Validators;

public interface IChatUserRuleValidator : ISoftDeletableValidator<ChatUserRule>
{
    public Result IsAvailableAndPermitted(ChatUserRule? chatUserRule);
}

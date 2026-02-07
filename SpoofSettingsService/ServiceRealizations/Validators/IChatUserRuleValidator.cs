using CommonObjects.Results;
using DataSaveHelpers.Services;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public interface IChatUserRuleValidator : ISoftDeletableValidator<ChatUserRule>
{
    public Result IsAvailableAndPermitted(ChatUserRule? chatUserRule);
}

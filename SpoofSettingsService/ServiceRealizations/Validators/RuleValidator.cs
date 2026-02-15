using CommonObjects.Results;
using CommunicationLibrary.Communication;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class RuleValidator : IRuleValidator
{
    public Result IsAvailableCollection(Rule[]? rules)
    {
        if (rules is null)
            return Result.ErrorResult("Can't find your rules");
        return Result.OkResult();
    }
}

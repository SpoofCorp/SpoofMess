using CommonObjects.Results;
using CommunicationLibrary.Communication;

namespace SpoofSettingsService.Services.Validators;

public interface IRuleValidator
{
    public Result IsAvailableCollection(Rule[]? rules);
}

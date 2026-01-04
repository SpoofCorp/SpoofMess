using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Validators;

public interface IUserValidator
{
    public Result Validate(User? user);
}

using CommonObjects.Results;
using DataHelpers;

namespace SpoofSettingsService.Services.Validators;

public interface IBaseValidator
{
    public Result ValidateExist<T>(T? entity) where T : ISoftDeletable;
}

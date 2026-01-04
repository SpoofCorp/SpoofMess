using CommonObjects.Results;
using DataHelpers;

namespace SpoofSettingsService.Services.Validators;

public class BaseValidator : IBaseValidator
{
    public Result ValidateExist<T>(T? entity) where T : ISoftDeletable
    {
        if (entity is null || entity.IsDeleted)
            return Result.BadRequest($"Invalid {typeof(T).Name.ToLower()}");

        return Result.SuccessResult();
    }
}
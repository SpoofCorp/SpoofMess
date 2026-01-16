using CommonObjects.Results;
using DataSaveHelpers;

namespace SpoofSettingsService.Services.Validators;

public interface IBaseValidator
{
    public Result ValidateExist<T>(T? entity) where T : ISoftDeletable;
}

using CommonObjects.Results;
using DataSaveHelpers.Services;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Validators;

public interface IUserValidator : ISoftDeletableValidator<User>
{
    public Result IsAvailableAndHisOwner(User? user, Guid userId);
}

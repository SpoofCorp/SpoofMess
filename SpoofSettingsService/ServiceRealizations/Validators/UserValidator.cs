using CommonObjects.Results;
using DataSaveHelpers.ServiceRealizations;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class UserValidator : SoftDeletableValidator<User>, IUserValidator
{
    public Result IsAvailableAndHisOwner(User? user, Guid userId)
    {
        Result result = IsAvailable(user);
        if (result.Success)
            return user!.Id == userId 
                ? Result.OkResult() 
                : Result.Forbidden("It's not your account");
        return result;
}
}
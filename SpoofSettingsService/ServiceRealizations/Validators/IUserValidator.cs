using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Validators;

public class UserValidator : IUserValidator
{
    public Result Validate(User? user)
    {
        if (user is null)
            return Result.BadRequest("Invalid id");

        if (user.IsDeleted)
            return Result.BadRequest("User was deleted");

        return Result.OkResult();
    }
}
using CommonObjects.Results;
using SpoofEntranceService.Models;
using SpoofEntranceService.Services.Validators;

namespace SpoofEntranceService.ServiceRealizations.Validators;

public class UserEntryValidator : IUserEntryValidator
{
    public Result IsActive(UserEntry? user)
    {
        if (user is null)
            return Result.NotFoundResult("User is not exist");
        if (user.IsDeleted)
            return Result.BadRequest($"User {user.Id} has been deleted");

        return Result.OkResult();
    }

    public Result HisIsActive(UserEntry? user)
    {
        if (user is null || user.IsDeleted)
            return Result.OkResult();

        return Result.BadRequest("Login is busy");
    }

}
using CommonObjects.Results;
using SpoofEntranceService.Models;

namespace SpoofEntranceService.Services.Validators;

public interface IUserEntryValidator
{
    public Result IsActive(UserEntry? user);

    public Result HisIsActive(UserEntry? user);
}

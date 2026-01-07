using CommonObjects.Results;
using DataHelpers;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class SoftDeletableValidator : ISoftDeletableValidator
{
    public Result IsNullOrDeleted<T>(T? obj) where T : ISoftDeletable
    {
        if (obj is null)
            return Result.NotFoundResult("Invalid id");
        if (obj.IsDeleted)
            return Result.BadRequest("Has been deleted");

        return Result.OkResult();
    }

    public Result IsNullOrEmptyCollection<T>(List<T>? objs)
    {
        if(objs is null)
            return Result.NotFoundResult("Invalid id");

        if (objs.Count == 0)
            return Result.BadRequest("Not such");

        return Result.OkResult();
    }
}

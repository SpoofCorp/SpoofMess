using CommonObjects.Results;
using DataHelpers;
using SpoofMessageService.Services.Validators;

namespace SpoofMessageService.ServiceRealizations.Validators;

public class SoftDeletableValidator<T> : ISoftDeletableValidator<T> where T : ISoftDeletable
{
    public Result IsAvailable(T? obj)
    {
        if (obj is null)
            return Result.NotFoundResult("Invalid id");
        if (obj.IsDeleted)
            return Result.BadRequest("Has been deleted");

        return Result.OkResult();
    }

    public Result IsAvailableCollection(List<T>? objs)
    {
        if(objs is null)
            return Result.NotFoundResult("Invalid id");

        if (objs.Count == 0)
            return Result.BadRequest("Not such");

        return Result.OkResult();
    }
}

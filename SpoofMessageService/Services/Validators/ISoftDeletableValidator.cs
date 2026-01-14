using CommonObjects.Results;
using DataHelpers;

namespace SpoofMessageService.Services.Validators;

public interface ISoftDeletableValidator<T> where T : ISoftDeletable
{
    public Result IsAvailable(T? obj);
    public Result IsAvailableCollection(List<T>? objs);
}

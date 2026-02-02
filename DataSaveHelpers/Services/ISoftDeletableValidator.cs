using CommonObjects.Results;
using DataSaveHelpers.EntityTypes;

namespace DataSaveHelpers.Services;

public interface ISoftDeletableValidator<T> where T : ISoftDeletable
{
    public Result IsAvailable(T? obj);
    public Result IsAvailableCollection(List<T>? objs);
}

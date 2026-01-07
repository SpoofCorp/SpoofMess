using CommonObjects.Results;
using DataHelpers;

namespace SpoofSettingsService.Services.Validators;

public interface ISoftDeletableValidator
{
    public Result IsNullOrDeleted<T>(T? obj) where T : ISoftDeletable;
    public Result IsNullOrEmptyCollection<T>(List<T>? objs);
}

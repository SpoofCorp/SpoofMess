using CommonObjects.Results;
using DataHelpers;

namespace SpoofSettingsService.Services.Validators;

public interface ISoftDeletableValidator
{
    public Result IsActive<T>(T? obj) where T : ISoftDeletable;
    public Result IsAvailableCollection<T>(List<T>? objs);
}

using CommonObjects.Requests;
using CommonObjects.Results;

namespace SpoofSettingsService.Services;

public interface IUserService
{
    public ValueTask<Result> ChangeSettings(ChangeUserSettingsRequest request, Guid userId);

    public ValueTask<Result> Delete(Guid userId);

    public ValueTask<Result> Create(Guid userId);
}

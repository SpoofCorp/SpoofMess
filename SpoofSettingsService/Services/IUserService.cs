using CommonObjects.Requests;
using CommonObjects.Results;
using CommunicationLibrary.Communication;

namespace SpoofSettingsService.Services;

public interface IUserService
{
    public ValueTask<Result> ChangeSettings(ChangeUserSettingsRequest request, Guid userId);

    public ValueTask<Result> Delete(Guid userId);

    public ValueTask<Result> Create(CreateUser createUser);
}

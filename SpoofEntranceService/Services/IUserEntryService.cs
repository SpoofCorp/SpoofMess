using CommonObjects.Requests;
using CommonObjects.Responses;
using CommonObjects.Results;
using SpoofEntranceService.Models;

namespace SpoofEntranceService.Services;

public interface IUserEntryService
{
    public Task<Result<UserAuthorizeResponse>> Authorization(UserAuthorizeRequest request, SessionInfo sessionInfo);

    [Obsolete("This method does not have the ability to correctly create an account in the entire system.", true)]
    public Task<Result<UserAuthorizeResponse>> Registration(RegistrationRequest request, SessionInfo sessionInfo);

    public Task<Result> Delete(SessionInfo sessionInfo);
    public Task Confirm(Guid userId);
    public Task Error(Guid userId);
    public Task Delete(Guid userId);
}

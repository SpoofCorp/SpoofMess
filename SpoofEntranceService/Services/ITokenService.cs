using CommonObjects.Requests;
using CommonObjects.Responses;
using CommonObjects.Results;
using SpoofEntranceService.Models;

namespace SpoofEntranceService.Services;

public interface ITokenService
{
    public Task<Result<UserAuthorizeResponse>> UpdateToken(UpdateTokenRequest tokenRequest);
    public Task<Result<TokenResponse>> Create(SessionInfo sessionInfo);
    public Task<Result<TokenResponse>> CreateAndSave(SessionInfo sessionInfo);
}

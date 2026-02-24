using CommonObjects.Results;
using SpoofEntranceService.Models;
using SpoofEntranceService.Services.Validators;

namespace SpoofEntranceService.ServiceRealizations.Validators;

public class SessionValidator : ISessionValidator
{
    public Result IsInvalidSession(SessionInfo? sessionInfo)
    {
        if (sessionInfo is null)
            return Result.NotFoundResult($"Not found your session");

        if (sessionInfo.IsDeleted || !sessionInfo.IsActive)
            return Result.BadRequest("Session is disabled or is deleted");

        return Result.OkResult();
    }

    public bool IsSessionTooNew(SessionInfo sessionInfo, DateTime now) =>
        sessionInfo.CreatedAt.Date >= now.AddDays(-7).Date;


    public Result ValidateTrustSession(SessionInfo? sessionInfo)
    {
        Result result = IsInvalidSession(sessionInfo);
        if (result.Success)
            result = IsSessionTooNew(sessionInfo!, DateTime.UtcNow) 
                ? Result.Forbidden("No trust") 
                : Result.OkResult();

        return result;
    }
}
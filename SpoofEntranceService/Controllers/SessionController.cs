using CommonObjects.DTO;
using CommonObjects.Requests;
using CommonObjects.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityLibrary;
using SpoofEntranceService.Services;

namespace SpoofEntranceService.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class SessionController(ISessionService sessionService) : ControllerBase
{
    private readonly ISessionService _sessionService = sessionService;

    [HttpPatch("Exit")]
    public async ValueTask<IActionResult> Exit(ExitRequest request)
    {
        Guid sessionId = ClaimService.GetSessionId(User);

        Result result = await _sessionService.Exit(request, sessionId);
        return StatusCode(
            result.StatusCode,
            result.Success
                ? result.Message
                : result.Error
            );
    }

    [HttpGet("GetSessions")]
    public async ValueTask<IActionResult> GetSessions()
    {
        Guid sessionId = ClaimService.GetSessionId(User);
        Guid userId = ClaimService.GetUserId(User);

        Result<List<SessionInfo>> result = await _sessionService.GetSessions(
                userId,
                sessionId
            );
        return StatusCode(
            result.StatusCode,
            result.Success
                ? result.Body
                : result.Error
            );
    }

    [HttpPatch("EndSessions")]
    public async ValueTask<IActionResult> EndSessions(bool withCurrent)
    {
        Guid sessionId = ClaimService.GetSessionId(User);

        Result result = await _sessionService.EndSessions(
                sessionId,
                withCurrent
            );
        return StatusCode(
            result.StatusCode,
            result.Success
                ? result.Message
                : result.Error
            );
    }

    [HttpPatch("EndSession")]
    public async ValueTask<IActionResult> EndSession(EndSessionRequest request)
    {
        Guid sessionId = ClaimService.GetSessionId(User);

        Result result = await _sessionService.EndSession(
                request,
                sessionId
            );
        return StatusCode(
            result.StatusCode,
            result.Success
                ? result.Message
                : result.Error
            );
    }
}

using CommonObjects.Requests.Avatars;
using CommonObjects.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityLibrary;
using SpoofSettingsService.Services;

namespace SpoofSettingsService.Controllers;

[Authorize]
[ApiController]
[Route("api/v2/[controller]")]
public class UserAvatarController(IUserAvatarService userAvatarService) : ControllerBase
{
    private readonly IUserAvatarService _userAvatarService = userAvatarService;

    [HttpPost("Set")]
    public async Task<IActionResult> Set(SesUserAvatarRequest request)
    {
        Guid userId = ClaimService.GetUserId(User);

        Result result = await _userAvatarService.SetAvatar(request, userId);
        return StatusCode(
            result.StatusCode,
            result.Success
                ? result.Message
                : result.Error
                );
    }
}

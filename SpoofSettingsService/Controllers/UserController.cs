using CommonObjects.Requests;
using CommonObjects.Results;
using Microsoft.AspNetCore.Mvc;
using SecurityLibrary;
using SpoofSettingsService.Services;

namespace SpoofSettingsService.Controllers;

[ApiController]
[Route("api/v2/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPatch("ChangeSettings")]
    public async Task<IActionResult> ChangeSettigns(ChangeUserSettingsRequest request)
    {
        Guid? userId = ClaimService.GetUserId(User);
        if (userId is null)
            return BadRequest("Invalid token");
        Result result = await _userService.ChangeSettings(request, userId.Value);
        return StatusCode(result.StatusCode, result.Success ? result.Message : result.Error);
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete()
    {
        Guid? userId = ClaimService.GetUserId(User);
        if (userId is null)
            return BadRequest("Invalid token");
        Result result = await _userService.Delete(userId.Value);
        return StatusCode(result.StatusCode, result.Success ? result.Message : result.Error);
    }
}

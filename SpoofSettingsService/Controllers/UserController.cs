using CommonObjects.DTO;
using CommonObjects.Requests.Changes;
using CommonObjects.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityLibrary;
using SpoofSettingsService.Services;

namespace SpoofSettingsService.Controllers;

[Authorize]
[ApiController]
[Route("api/v2/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPatch("ChangeSettings")]
    public async Task<IActionResult> ChangeSettigns(ChangeUserSettingsRequest request)
    {
        Guid userId = ClaimService.GetUserId(User);

        Result result = await _userService.ChangeSettings(request, userId);
        return StatusCode(
            result.StatusCode,
            result.Success
                ? result.Message
                : result.Error
                );
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete()
    {
        Guid userId = ClaimService.GetUserId(User);

        Result result = await _userService.Delete(userId);
        return StatusCode(
            result.StatusCode,
            result.Success
                ? result.Message
                : result.Error
                );
    }
    [HttpGet("info")]
    public async Task<IActionResult> GetInfo(string login)
    {
        Guid userId = ClaimService.GetUserId(User);

        Result<UserDTO> result = await _userService.GetInfo(login, userId);
        return StatusCode(
            result.StatusCode,
            result.Success
                ? result.Body
                : result.Error
                );

    }
}

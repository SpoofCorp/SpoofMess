using CommonObjects.Results;
using Microsoft.AspNetCore.Mvc;
using SpoofMessageService.Services;

namespace SpoofMessageService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("stat")]
    public async Task<IActionResult> Stat()
    {
        Result<int> result = await _userService.Stat();
        return StatusCode(
            result.StatusCode,
            result
                ? result.Body
                : result.Error
            );
    }
}
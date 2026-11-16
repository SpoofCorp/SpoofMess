using CommonObjects.DTO;
using CommonObjects.Requests.Stickers;
using CommonObjects.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityLibrary;
using SpoofSettingsService.Services;

namespace SpoofSettingsService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserStickerPackController(IUserStickerPackService userStickerPackService) : ControllerBase
{
    private readonly IUserStickerPackService _userStickerPackService = userStickerPackService;

    [HttpPost("Get/Many")]
    public async Task<IActionResult> GetMany(GetStickerPackRequest request)
    {
        Guid userId = ClaimService.GetUserId(User);
        Result<IEnumerable<UserStickerPackDTO>> result = await _userStickerPackService.GetUserStickerPacks(userId);
        return StatusCode(result.StatusCode, result);
    }
}

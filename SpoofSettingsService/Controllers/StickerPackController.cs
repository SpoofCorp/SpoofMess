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
public class StickerPackController(IStickerPackService stickerPackService) : ControllerBase
{
    private readonly IStickerPackService _stickerPackService = stickerPackService;

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateStickerPackRequest request)
    {
        Guid userId = ClaimService.GetUserId(User);
        Result<StickerPackDTO> result = await _stickerPackService.CreateStickerPackAsync(request, userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Get")]
    public async Task<IActionResult> Get(GetStickerPackRequest request)
    {
        Result<StickerPackDTO> result = await _stickerPackService.GetStickerPackAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Get/Many")]
    public async Task<IActionResult> GetMany(GetStickerPackRequest request)
    {
        Result<StickerPackDTO> result = await _stickerPackService.GetStickerPackAsync(request);
        return StatusCode(result.StatusCode, result);
    }
}
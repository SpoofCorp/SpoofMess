using CommonObjects.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityLibrary;
using SpoofFileService.Services;

namespace SpoofFileService.Controllers;


[Authorize]
[ApiController]
[Route("api/v3/[controller]")]
public class FileController(IFileService fileService) : ControllerBase
{
    private readonly IFileService _fileService = fileService;

    [HttpGet("upload")]
    public async Task<IActionResult> Upload(byte[] fileId)
    {
        Guid userId = ClaimService.GetUserId(User);
        Result<FileStream> result = await _fileService.GetFile(fileId, userId);

        return StatusCode(
            result.StatusCode,
            result.Success
                ? result.Body
                : result.Error
                );
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save(IFormFile file)
    {
        Result<byte[]> result = await _fileService.SaveFile(file);

        return StatusCode(
            result.StatusCode,
            result.Success
                ? result.Body
                : result.Error
                );
    }
}

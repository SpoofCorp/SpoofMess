using CommonObjects.Requests.Files;
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

    [HttpGet("first-check")]
    public async Task<IActionResult> CheckL1(FingerprintExistL1L2 request)
    {
        Result result = await _fileService.ExistL1(request);
        return StatusCode(result.StatusCode, result.Success
            ? "L1 access granted" 
            : result.Error);
    }
    [HttpGet("full-check")]
    public async Task<IActionResult> CheckL3(FingerprintExistL3 request)
    {
        Guid userId = ClaimService.GetUserId(User);
        Result<byte[]> result = await _fileService.ExistL3(request, userId);
        return StatusCode(result.StatusCode, result.Success
            ? result.Body
            : result.Error);
    }

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
        Guid userId = ClaimService.GetUserId(User);
        Result<byte[]> result = await _fileService.SaveFile(file, userId);

        return StatusCode(
            result.StatusCode,
            result.Success
                ? result.Body
                : result.Error
                );
    }
}

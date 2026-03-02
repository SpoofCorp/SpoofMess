using CommonObjects.DTO;
using CommonObjects.Requests.Members;
using CommonObjects.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityLibrary;
using SpoofSettingsService.Services;

namespace SpoofSettingsService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ChatUserController(
    IChatUserService chatUserService
    ) : ControllerBase
{
    private readonly IChatUserService _chatUserService = chatUserService;

    [HttpPost("addMember")]
    public async Task<IActionResult> AddMember(AddMemberRequest request)
    {
        Guid userId = ClaimService.GetUserId(User);

        Result result = await _chatUserService.Add(request, userId);
        return StatusCode(
            result.StatusCode,
            result.Success 
                ? result.Message
                : result.Error
                );
    }

    [HttpPost("join")]
    public async Task<IActionResult> Join(JoinToChatRequest request)
    {
        Guid userId = ClaimService.GetUserId(User);

        Result result = await _chatUserService.Join(request, userId);
        return StatusCode(
            result.StatusCode,
            result.Success
                ? result.Message
                : result.Error
                );
    }

    [HttpGet("get-chats")]
    public async Task<IActionResult> GetChats(DateTime after)
    {
        Guid userId = ClaimService.GetUserId(User);
        if(after.Kind != DateTimeKind.Utc)
        {
            after = after.ToUniversalTime();
        }
        Result<List<ChatUserDTO>> result = await _chatUserService.GetUserChats(userId, after);
        return StatusCode(
            result.StatusCode,
            result.Success
                ? result.Body
                : result.Error
                );
    }
}

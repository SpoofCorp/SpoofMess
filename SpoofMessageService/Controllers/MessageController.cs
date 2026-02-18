using CommonObjects.Requests.Messages;
using CommonObjects.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityLibrary;
using SpoofMessageService.Services;

namespace SpoofMessageService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MessageController(IMessageService messageService) : ControllerBase
{
    private readonly IMessageService _messageService = messageService;

    [HttpPost]
    public async Task<IActionResult> Send(CreateMessageRequest request)
    {
        Guid? userId = ClaimService.GetUserId(User);
        if (userId == null)
            return Unauthorized("Invalid token");

        Result result = await _messageService.SendMessage(request, userId.Value);
        return StatusCode(result.StatusCode, result.Success ? result.Message : result.Error);
    }
}

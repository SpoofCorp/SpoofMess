using CommonObjects.Requests.Messages;
using CommonObjects.Results;
using Microsoft.AspNetCore.SignalR;
using SecurityLibrary;
using SpoofMessageService.Services;
using System.Security.Claims;

namespace SpoofMessageService;

public class ChatHub(
        IMessageService messageService,
        IUserService userService
    ) : Hub
{
    private readonly IMessageService _messageService = messageService;
    private readonly IUserService _userService = userService;

    public override async Task OnConnectedAsync()
    {
        Guid userId = GetUserId(Context.User);

        await _userService.ChangeConnectionState(userId, true);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Guid userId = GetUserId(Context.User);

        await _userService.ChangeConnectionState(userId, false);

        await base.OnDisconnectedAsync(exception);
    }


    public async Task SendMessage(CreateMessageRequest request)
    {
        Guid userId = GetUserId(Context.User);

        Result result = await _messageService.SendMessage(request, userId);
    }

    public async Task DeleteMessage(DeleteMessageRequest request)
    {
        Guid userId = GetUserId(Context.User);

        Result result = await _messageService.DeleteMessage(request, userId);
    }

    public async Task EditMessage(EditMessageRequest request)
    {
        Guid userId = GetUserId(Context.User);

        Result result = await _messageService.EditMessage(request, userId);
    }

    private static Guid GetUserId(ClaimsPrincipal? user) =>
        ClaimService.GetUserId(user) ?? throw new HubException("User is not authenticated.");
}

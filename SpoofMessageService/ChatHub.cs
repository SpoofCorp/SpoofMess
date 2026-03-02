using CommonObjects.DTO;
using CommonObjects.Requests.Messages;
using CommonObjects.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SecurityLibrary;
using SpoofMessageService.Models;
using SpoofMessageService.Services;
using System.Collections.Concurrent;

namespace SpoofMessageService;

[Authorize]
public class ChatHub(
        IMessageService messageService,
        IUserService userService,
        IChatUserService chatUserService
    ) : Hub
{
    private readonly IChatUserService _chatUserService = chatUserService;
    private readonly IMessageService _messageService = messageService;
    private readonly IUserService _userService = userService;
    private readonly ConcurrentDictionary<Guid, List<UserConnection>> Users = [];
    public override async Task OnConnectedAsync()
    {
        Guid userId = ClaimService.GetUserId(Context.User);
        Guid sessionId = ClaimService.GetSessionId(Context.User);

        await _userService.ChangeConnectionState(userId, true);
        if (Users.TryGetValue(userId, out List<UserConnection>? sessions))
            sessions.Add(new(Context.ConnectionId, sessionId));
        else
            Users.TryAdd(userId, [new(Context.ConnectionId, sessionId)]);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Guid userId = ClaimService.GetUserId(Context.User);
        Guid sessionId = ClaimService.GetSessionId(Context.User);

        await _userService.ChangeConnectionState(userId, false);
        if (Users.TryGetValue(userId, out List<UserConnection>? sessions))
            sessions.Remove(new(Context.ConnectionId, sessionId));

        await base.OnDisconnectedAsync(exception);
    }


    public async Task SendMessage(CreateMessageRequest request)
    {
        Guid userId = ClaimService.GetUserId(Context.User);

        Result<MessageDTO> result = await _messageService.SendMessage(request, userId);
        _ = Task.Run(async () =>
        {
            if (!result.Success)
                return;
            Result<List<ChatUser>> users = await _chatUserService.GetMembers(result.Body!.ChatId);
            if (users.Success)
                await Parallel.ForEachAsync(users.Body!, async (user, token) =>
                {
                    if (Users.TryGetValue(user.Key2, out List<UserConnection>? connections))
                    {
                        foreach (var connection in connections)
                        {
                            await Clients.Client(connection.Ip).SendAsync("new-message", result.Body, token);
                        }
                    }
                });
        });
    }

    public async Task DeleteMessage(DeleteMessageRequest request)
    {
        Guid userId = ClaimService.GetUserId(Context.User);

        Result result = await _messageService.DeleteMessage(request, userId);
    }

    public async Task EditMessage(EditMessageRequest request)
    {
        Guid userId = ClaimService.GetUserId(Context.User);

        Result result = await _messageService.EditMessage(request, userId);
    }
}

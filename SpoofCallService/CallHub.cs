using AdditionalHelpers.Services;
using CommonObjects.Results;
using Microsoft.AspNetCore.SignalR;
using SecurityLibrary;
using SpoofCallService.Models;
using SpoofCallService.Services;
using System.Collections.Concurrent;

namespace SpoofCallService;

public class CallHub(
    ConnectionTracker tracker,
    IChatUserService chatUserService,
    ILoggerService logger) : Hub
{
    private readonly ConnectionTracker _tracker = tracker;
    private readonly IChatUserService _chatUserService = chatUserService;
    private readonly ILoggerService _logger = logger;

    public override async Task OnConnectedAsync()
    {
        (Guid sessionId, Guid userId) = ClaimService.GetUserInfo(Context.User);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
        Result<IEnumerable<ChatUser>> chatUsers = await _chatUserService.GetUserChats(userId);
        if (chatUsers)
            foreach (var chatUser in chatUsers.Body!)
                await Groups.AddToGroupAsync(Context.ConnectionId, $"chat-{chatUser.Chat.UniqueName}");

        await base.OnConnectedAsync();
    }

    public async Task StartCall()
    {

    }

    public async Task EndCall()
    {

    }


    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        (Guid sessionId, Guid userId) = ClaimService.GetUserInfo(Context.User);

        if (_tracker.Get(userId, out ConcurrentDictionary<Guid, UserConnection>? sessions))
            sessions!.TryRemove(sessionId, out _);

        await base.OnDisconnectedAsync(exception);
    }

}

public class CallsStorage
{
    private readonly static ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, UserConnection>> Users = [];

    public bool Get(Guid userId, out ConcurrentDictionary<Guid, UserConnection>? user) =>
        Users.TryGetValue(userId, out user);

    public bool Add(Guid userId, ConcurrentDictionary<Guid, UserConnection> user) =>
        Users.TryAdd(userId, user);
}

public class ChatRooms
{
    private readonly static ConcurrentDictionary<string, CallingRoom> Rooms = [];

    /*public bool TryAdd(Guid userId, CallMemberSession callMemberSession, CallMember callMember)
    {
        
        Client client = _members.GetOrAdd(userId, (id) => new Client(callMember));
        return client.Devices.TryAdd(callMemberSession.DeviceId, callMemberSession);
    }*/

}

public class CallingRoom
{
    public readonly static ConcurrentDictionary<Guid, Client> _members = [];

    public bool TryAdd(Guid userId, CallMemberSession callMemberSession, CallMember callMember)
    {
        Client client = _members.GetOrAdd(userId, (id) => new Client(callMember));
        return client.Devices.TryAdd(callMemberSession.DeviceId, callMemberSession);
    }
}

public class Client(CallMember callMember)
{
    public CallMember CallMember { get; set; } = callMember;
    public ConcurrentDictionary<string, CallMemberSession> Devices { get; set; } = [];
}

public class ConnectionTracker
{
    private readonly static ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, UserConnection>> Users = [];

    public bool Get(Guid userId, out ConcurrentDictionary<Guid, UserConnection>? user) =>
        Users.TryGetValue(userId, out user);

    public bool Add(Guid userId, ConcurrentDictionary<Guid, UserConnection> user) =>
        Users.TryAdd(userId, user);
}
public record class UserConnection(
    string Ip,
    Guid SessionId
);

public class CallRoom(Chat chat, Call call)
{
    public List<User> Members { get; set; } = [];

    public Chat Chat { get; set; } = chat;

    public Call Call { get; set; } = call;
}
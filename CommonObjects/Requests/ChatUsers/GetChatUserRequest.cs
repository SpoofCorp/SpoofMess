namespace CommonObjects.Requests.ChatUsers;

public record class GetChatUserRequest(
    Guid ChatId, 
    Guid UserId
    );

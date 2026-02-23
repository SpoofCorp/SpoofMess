namespace CommunicationLibrary.Communication;

public record DeleteChatUser(
    Guid UserId,
    Guid ChatId
    );

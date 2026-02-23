namespace CommunicationLibrary.Communication;

public record UpdateChatUser(
    Guid UserId,
    Guid ChatId,
    Rule[] Rules
    );

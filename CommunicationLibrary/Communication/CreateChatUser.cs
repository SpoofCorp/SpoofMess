namespace CommunicationLibrary.Communication;

public record CreateChatUser(
    Guid UserId, 
    Guid ChatId, 
    Rule[] Rules
    );

namespace CommunicationLibrary.Communication;

public record CreateChat(
    Guid Id, 
    Guid? AvatarId,
    string UniqueName,
    string Name
    );
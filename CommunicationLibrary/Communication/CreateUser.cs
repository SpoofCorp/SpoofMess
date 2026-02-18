namespace CommunicationLibrary.Communication;

public record CreateUser(
    Guid UserId,
    string Name,
    string Login
    );
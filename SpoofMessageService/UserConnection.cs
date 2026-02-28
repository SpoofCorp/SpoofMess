namespace SpoofMessageService;

public record UserConnection(
        string Ip,
        Guid SessionId
    );

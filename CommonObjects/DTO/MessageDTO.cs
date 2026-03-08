namespace CommonObjects.DTO;

public record MessageDTO(
        Guid Id,
        Guid ChatId,
        Guid UserId,
        string UserName,
        byte[]? UserAvatarId,
        string Text,
        DateTime SendAt,
        List<byte[]> Attachments
    );

namespace CommonObjects.DTO;

public record MessageDTO(
        Guid Id,
        Guid ChatId,
        Guid UserId,
        string Text,
        DateTime SendAt
    );

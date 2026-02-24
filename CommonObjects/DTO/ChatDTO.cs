namespace CommonObjects.DTO;

public record ChatDTO(
        Guid Id,
        int ChatTypeId,
        string UniqueName,
        string Name,
        DateTime CreatedAt,
        Guid? OwnerId
    );
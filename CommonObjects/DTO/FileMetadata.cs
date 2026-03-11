namespace CommonObjects.DTO;

public record FileMetadata(
        Guid Id,
        string OriginalName,
        long Size);
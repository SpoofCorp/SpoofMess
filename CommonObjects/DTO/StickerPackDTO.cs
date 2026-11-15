namespace CommonObjects.DTO;

public record StickerPackDTO(
        FileMetadata FileMetadata,
        string Key,
        string? Title,
        string Author,
        IEnumerable<StickerDTO>? Stickers
    );

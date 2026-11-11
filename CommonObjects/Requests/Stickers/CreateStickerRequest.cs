using CommonObjects.DTO;

namespace CommonObjects.Requests.Stickers;

public record class CreateStickerRequest(
        string StickerPackKey,
        FileMetadata FileMetadata,
        string Name,
        string Key
    );
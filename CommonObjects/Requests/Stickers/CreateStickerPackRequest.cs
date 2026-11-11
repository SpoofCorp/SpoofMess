using CommonObjects.DTO;

namespace CommonObjects.Requests.Stickers;

public record class CreateStickerPackRequest(
        FileMetadata FileMetadata,
        string Name,
        string Key
    );
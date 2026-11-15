namespace CommonObjects.Requests.Stickers;

public record class DeleteStickerRequest(
        string StickerPackKey,
        string StickerKey
    );
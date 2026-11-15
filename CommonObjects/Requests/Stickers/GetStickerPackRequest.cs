namespace CommonObjects.Requests.Stickers;

public record class GetStickerPackRequest(
        string Key,
        bool WithStickers
    );
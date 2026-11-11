namespace CommonObjects.Requests.Stickers;

public record class GetStickerPackRequest(
        long Id,
        bool WithStickers
    );
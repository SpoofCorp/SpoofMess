namespace CommonObjects.Requests.Stickers;

public class GetStickerPackRequest
{
    public Guid Id { get; set; }

    public bool WithStickers { get; set; }
}

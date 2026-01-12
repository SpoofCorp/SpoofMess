namespace CommonObjects.Requests.Stickers;

public class DeleteStickerRequest
{
    public long StickerPackId { get; set; }
    public Guid StickerId { get; set; }
}

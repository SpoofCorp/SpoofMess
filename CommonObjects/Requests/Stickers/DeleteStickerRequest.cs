namespace CommonObjects.Requests.Stickers;

public class DeleteStickerRequest
{
    public Guid StickerPackId { get; set; }
    public Guid StickerId { get; set; }
}

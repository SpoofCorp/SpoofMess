using CommonObjects.DTO;

namespace CommonObjects.Requests.Stickers;

public class CreateStickerRequest
{
    public Guid StickerPackId { get; set; }

    public required FileMetadata FileMetadata { get; set; }

    public string StickerName { get; set; } = string.Empty;

}

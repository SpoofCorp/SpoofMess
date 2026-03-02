using CommonObjects.DTO;

namespace CommonObjects.Responses;

public class GetStickerPackResponse
{
    public byte[] FileId { get; set; } = null!;

    public List<StickerDTO>? Stickers { get; set; }
}

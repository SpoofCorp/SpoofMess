using CommonObjects.DTO;

namespace CommonObjects.Responses;

public class GetStickerPackResponse
{
    public Guid FileId { get; set; }

    public List<StickerDTO>? Stickers { get; set; }
}

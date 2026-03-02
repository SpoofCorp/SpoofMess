namespace CommonObjects.DTO;

public class StickerDTO
{
    public byte[] FileId { get; set; } = null!;

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}

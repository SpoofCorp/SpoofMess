using DataHelpers;

namespace SpoofSettingsService.Models;

public partial class Sticker : IdentifiedEntity<Guid>, IChangeable
{
    public Guid StickerPackId { get; set; }

    public Guid? FileId { get; set; }

    public string Title { get; set; } = null!;

    public DateTime LastModified { get; set; }

    public virtual FileMetadatum? File { get; set; }

    public virtual StickerPack StickerPack { get; set; } = null!;
}

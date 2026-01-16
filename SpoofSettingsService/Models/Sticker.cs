using DataSaveHelpers;

namespace SpoofSettingsService.Models;

public partial class Sticker : IdentifiedSoftDeletableEntity<Guid>
{
    public long StickerPackId { get; set; }

    public Guid FileId { get; set; }

    public string Title { get; set; } = null!;

    public DateTime LastModified { get; set; }

    public virtual FileMetadatum File { get; set; } = null!;

    public virtual StickerPack StickerPack { get; set; } = null!;
}

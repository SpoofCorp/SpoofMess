using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofSettingsService.Models;

public partial class Sticker : IdentifiedSoftDeletableChangeableEntity<Guid>
{
    public long StickerPackId { get; set; }

    public Guid FileId { get; set; }

    public string Title { get; set; } = null!;

    public virtual FileMetadatum File { get; set; } = null!;

    public virtual StickerPack StickerPack { get; set; } = null!;
}

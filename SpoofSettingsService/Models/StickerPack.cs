using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofSettingsService.Models;

public partial class StickerPack : IdentifiedSoftDeletableChangeableEntity<long>
{
    public Guid AuthorId { get; set; }

    public Guid PreviewId { get; set; }

    public string? Title { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual FileMetadatum Preview { get; set; } = null!;

    public virtual ICollection<Sticker> Stickers { get; set; } = [];
}

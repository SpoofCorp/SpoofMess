using DataHelpers;

namespace SpoofSettingsService.Models;

public partial class StickerPack : IdentifiedEntity<Guid>, IChangeable
{
    public Guid? AuthorId { get; set; }

    public string Title { get; set; } = null!;

    public Guid? PreviewId { get; set; }

    public DateTime LastModified { get; set; }

    public virtual User? Author { get; set; }

    public virtual FileMetadatum? Preview { get; set; }

    public virtual ICollection<Sticker> Stickers { get; set; } = [];
}

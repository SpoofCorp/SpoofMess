using DataSaveHelpers;

namespace SpoofSettingsService.Models;

public partial class ChatAvatar : IdentifiedSoftDeletableEntity<Guid>, IChangeable
{
    public Guid ChatId { get; set; }

    public Guid FileId { get; set; }

    public bool IsActive { get; set; }

    public DateTime LastModified { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual FileMetadatum File { get; set; } = null!;
}

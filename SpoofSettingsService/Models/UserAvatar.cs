using DataSaveHelpers;

namespace SpoofSettingsService.Models;

public partial class UserAvatar : SoftDeletableEntity
{
    public Guid UserId { get; set; }

    public Guid FileId { get; set; }

    public bool IsActive { get; set; }

    public DateTime LastModified { get; set; }

    public virtual FileMetadatum File { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
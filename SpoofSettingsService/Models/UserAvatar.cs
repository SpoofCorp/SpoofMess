using DataHelpers;

namespace SpoofSettingsService.Models;

public partial class UserAvatar : IdentifiedEntity<Guid>, IChangeable
{
    public Guid? UserId { get; set; }

    public Guid? FileId { get; set; }

    public bool IsActive { get; set; }

    public DateTime LastModified { get; set; }

    public virtual FileMetadatum? File { get; set; }

    public virtual User? User { get; set; }
}

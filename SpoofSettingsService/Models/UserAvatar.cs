using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofSettingsService.Models;

public partial class UserAvatar : IdentifiedSoftDeletableChangeableEntity<Guid>
{    
    public Guid UserId { get; set; }

    public Guid FileId { get; set; }

    public string OriginalFileName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual FileMetadatum File { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
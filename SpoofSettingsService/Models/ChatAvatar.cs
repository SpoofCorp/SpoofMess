using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofSettingsService.Models;

public partial class ChatAvatar : IdentifiedSoftDeletableChangeableEntity<Guid>
{
    public Guid ChatId { get; set; }

    public Guid FileId { get; set; }

    public bool IsActive { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual FileMetadatum File { get; set; } = null!;
}

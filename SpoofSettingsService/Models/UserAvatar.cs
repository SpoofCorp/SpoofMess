using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpoofSettingsService.Models;

public partial class UserAvatar : DoubleIdentifiedSoftDeletableChangeableEntity<Guid, Guid>
{    /// <summary>
     /// UserId
     /// </summary>
    [Column("UserId")]
    public new Guid Key1 { get; set; }

    /// <summary>
    /// FileId
    /// </summary>
    [Column("FileId")]
    public new Guid Key2 { get; set; }

    public string OriginalFileName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual FileMetadatum File { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
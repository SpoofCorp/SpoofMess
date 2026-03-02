using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpoofSettingsService.Models;

public partial class ChatAvatar : DoubleIdentifiedSoftDeletableChangeableEntity<Guid, byte[]>
{
    /// <summary>
    /// ChatId
    /// </summary>
    [Column("ChatId")]
    public new Guid Key1 { get; set; }

    /// <summary>
    /// FileId
    /// </summary>
    [Column("FileId")]
    public new byte[] Key2 { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual FileMetadatum File { get; set; } = null!;
}

using CommonObjects.DTO;
using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpoofMessageService.Models;

public partial class Attachment : DoubleIdentifiedSoftDeletableChangeableEntity<Guid, Guid>
{
    /// <summary>
    /// MessageId
    /// </summary>
    [Column("MessageId")]
    public new Guid Key1 { get; set; }
    /// <summary>
    /// FileMetadataId
    /// </summary>

    [Column("FileMetadataId")]
    public new Guid Key2 { get; set; }

    public string OriginalFileName { get; set; } = null!;

    public virtual FileMetadatum FileMetadata { get; set; } = null!;

    public virtual Message Message { get; set; } = null!;

    [NotMapped]
    public long Size { get; set; }
    [NotMapped]
    public string Category { get; set;  } = string.Empty;
}

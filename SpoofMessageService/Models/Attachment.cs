using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpoofMessageService.Models;

public partial class Attachment : DoubleIdentifiedSoftDeletableChangeableEntity<Guid, byte[]>
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
    public new byte[] Key2 { get; set; } = null!;

    public string OriginalFileName { get; set; } = null!;

    public virtual ICollection<AttachmentOperationStatus> AttachmentOperationStatuses { get; set; } = [];

    public virtual FileMetadatum FileMetadata { get; set; } = null!;

    public virtual Message Message { get; set; } = null!;
}

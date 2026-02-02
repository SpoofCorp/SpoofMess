using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpoofMessageService.Models;

public partial class Attachment : DoubleIdentifiedSoftDeletableChangeableEntity<Guid, Guid>
{
    [Column("MessageId")]
    public new Guid Key1 { get; set; }

    [Column("FileMetadataId")]
    public new Guid Key2 { get; set; }

    public virtual ICollection<AttachmentOperationStatus> AttachmentOperationStatuses { get; set; } = [];

    public virtual FileMetadatum FileMetadata { get; set; } = null!;

    public virtual Message Message { get; set; } = null!;
}

using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofMessageService.Models;

public partial class AttachmentOperationStatus : IdentifiedEntity<long>
{
    public Guid MessageId { get; set; }

    public Guid FileMetadataId { get; set; }

    public short OperationStatusId { get; set; }

    public string? Description { get; set; }

    public DateTime TimeSet { get; set; }

    public bool IsActual { get; set; }

    public virtual Attachment Attachment { get; set; } = null!;

    public virtual OperationStatus OperationStatus { get; set; } = null!;
}

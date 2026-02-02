using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofMessageService.Models;

public partial class OperationStatus : IdentifiedEntity<short>
{
    public string Name { get; set; } = null!;

    public virtual ICollection<AttachmentOperationStatus> AttachmentOperationStatuses { get; set; } = [];

    public virtual ICollection<MessageOperationStatus> MessageOperationStatuses { get; set; } = [];
}
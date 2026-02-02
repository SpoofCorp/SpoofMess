using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofMessageService.Models;

public partial class MessageOperationStatus : IdentifiedEntity<long>
{
    public Guid MessageId { get; set; }

    public short OperationStatusId { get; set; }

    public string? Description { get; set; }

    public DateTime TimeSet { get; set; }

    public bool IsActual { get; set; }

    public virtual Message Message { get; set; } = null!;

    public virtual OperationStatus OperationStatus { get; set; } = null!;
}

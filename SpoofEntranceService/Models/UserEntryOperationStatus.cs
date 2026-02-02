using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofEntranceService.Models;

public partial class UserEntryOperationStatus : IdentifiedEntity<long>
{
    public Guid UserEntryId { get; set; }

    public short OperationStatusId { get; set; }

    public string? Description { get; set; }

    public DateTime TimeSet { get; set; }

    public bool IsActual { get; set; }

    public virtual OperationStatus OperationStatus { get; set; } = null!;

    public virtual UserEntry UserEntry { get; set; } = null!;
}

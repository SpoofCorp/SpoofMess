using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofEntranceService.Models;

public partial class UserEntryOutbox : IdentifiedEntity<Guid>
{
    public Guid UserEntryId { get; set; }

    public bool IsSynced { get; set; }

    public DateTime LastTryDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Data { get; set; } = null!;

    public virtual UserEntry UserEntry { get; set; } = null!;
}

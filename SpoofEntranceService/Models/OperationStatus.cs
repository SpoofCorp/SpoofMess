using DataSaveHelpers;
using System;
using System.Collections.Generic;

namespace SpoofEntranceService.Models;

public partial class OperationStatus : IdentifiedEntity<short>
{
    public string Name { get; set; } = null!;

    public virtual ICollection<UserEntryOperationStatus> UserEntryOperationStatuses { get; set; } = new List<UserEntryOperationStatus>();
}

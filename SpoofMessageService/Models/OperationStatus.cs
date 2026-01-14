using System;
using System.Collections.Generic;

namespace SpoofMessageService.Models;

public partial class OperationStatus
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AttachmentOperationStatus> AttachmentOperationStatuses { get; set; } = new List<AttachmentOperationStatus>();

    public virtual ICollection<MessageOperationStatus> MessageOperationStatuses { get; set; } = new List<MessageOperationStatus>();
}

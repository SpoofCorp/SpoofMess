using System;
using System.Collections.Generic;

namespace SpoofMessageService.Models;

public partial class ViewMessage
{
    public Guid UserId { get; set; }

    public Guid MessageId { get; set; }

    public DateTime ViewTime { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime LastModified { get; set; }

    public virtual Message Message { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace SpoofMessageService.Models;

public partial class Attachment
{
    public Guid MessageId { get; set; }

    public Guid FileMetadataId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime LastModified { get; set; }

    public virtual ICollection<AttachmentOperationStatus> AttachmentOperationStatuses { get; set; } = new List<AttachmentOperationStatus>();

    public virtual FileMetadatum FileMetadata { get; set; } = null!;

    public virtual Message Message { get; set; } = null!;
}

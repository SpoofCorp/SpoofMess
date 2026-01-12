using System;
using System.Collections.Generic;

namespace SpoofMessageService.Models;

public partial class FileMetadatum
{
    public Guid Id { get; set; }

    public long Size { get; set; }

    public bool IsDeleted { get; set; }

    public short ExtensionId { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual Extension Extension { get; set; } = null!;
}

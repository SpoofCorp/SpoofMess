using System;
using System.Collections.Generic;

namespace SpoofMessageService.Models42;

public partial class FileObject
{
    public Guid Id { get; set; }

    public byte[]? L1 { get; set; }

    public byte[]? L2 { get; set; }

    public byte[] L3 { get; set; } = null!;

    public short ExtensionId { get; set; }

    public bool IsDeleted { get; set; }

    public string Path { get; set; } = null!;

    public long Size { get; set; }

    public DateTime LastModified { get; set; }

    public virtual Extension Extension { get; set; } = null!;
}

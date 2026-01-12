using System;
using System.Collections.Generic;

namespace SpoofMessageService.Models;

public partial class Extension
{
    public short Id { get; set; }

    public short FileCategory { get; set; }

    public bool IsDeleted { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<FileMetadatum> FileMetadata { get; set; } = new List<FileMetadatum>();
}

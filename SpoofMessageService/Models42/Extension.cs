using System;
using System.Collections.Generic;

namespace SpoofMessageService.Models42;

public partial class Extension
{
    public short Id { get; set; }

    public short CategoryId { get; set; }

    public bool IsDeleted { get; set; }

    public string Name { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<FileObject> FileObjects { get; set; } = new List<FileObject>();
}

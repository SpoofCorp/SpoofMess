using System;
using System.Collections.Generic;

namespace SpoofMessageService.Models42;

public partial class Category
{
    public short Id { get; set; }

    public bool IsDeleted { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Extension> Extensions { get; set; } = new List<Extension>();
}

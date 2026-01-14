using System;
using System.Collections.Generic;

namespace SpoofMessageService.Models;

public partial class FileType
{
    public short Id { get; set; }

    public bool IsDeleted { get; set; }

    public string Name { get; set; } = null!;
}

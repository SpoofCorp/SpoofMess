using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofFileService.Models;

public partial class FileObject : IdentifiedSoftDeletableChangeableEntity<byte[]>
{
    public byte[]? L1 { get; set; }

    public byte[]? L2 { get; set; }

    public short CategoryId { get; set; }

    public short ExtensionId { get; set; }

    public string Path { get; set; } = null!;

    public long Size { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Extension Extension { get; set; } = null!;
}

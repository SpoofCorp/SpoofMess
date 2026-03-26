using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofFileService.Models;

public partial class FileObject : IdentifiedSoftDeletableChangeableEntity<Guid>
{
    public byte[]? L1 { get; set; }

    public byte[]? L2 { get; set; }

    public byte[] L3 { get; set; } = null!;

    public short ExtensionId { get; set; }

    public string Path { get; set; } = null!;

    public string? Metadata { get; set; }

    public long Size { get; set; }

    public virtual Extension Extension { get; set; } = null!;
}

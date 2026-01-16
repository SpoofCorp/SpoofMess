using DataSaveHelpers;

namespace SpoofFileService.Models;

public partial class FileObject : IdentifiedSoftDeletableEntity<Guid>
{
    public string FilePath { get; set; } = null!;

    public DateTime LastModified { get; set; }
}

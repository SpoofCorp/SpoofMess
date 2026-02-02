using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofFileService.Models;

public partial class FileObject : IdentifiedSoftDeletableChangeableEntity<Guid>
{
    public string FilePath { get; set; } = null!;
}

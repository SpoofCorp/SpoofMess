using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofFileService.Models;

public partial class Extension : IdentifiedSoftDeletableEntity<short>
{
    public string? Name { get; set; }

    public virtual ICollection<FileObject> FileObjects { get; set; } = [];
}

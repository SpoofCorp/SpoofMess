using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofFileService.Models;

public partial class Extension : IdentifiedSoftDeletableEntity<short>
{
    public string Name { get; set; } = null!;

    public short CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<FileObject> FileObjects { get; set; } = [];
}

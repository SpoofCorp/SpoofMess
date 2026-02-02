using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofMessageService.Models;

public partial class Extension : IdentifiedSoftDeletableEntity<short>
{
    public short FileCategory { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<FileMetadatum> FileMetadata { get; set; } = [];
}

using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofFileService.Models;

public partial class Category : IdentifiedSoftDeletableEntity<short>
{
    public string Name { get; set; } = null!;

    public virtual ICollection<Extension> Extensions { get; set; } = [];
}

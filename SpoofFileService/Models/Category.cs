using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofFileService.Models;

public partial class Category : IdentifiedSoftDeletableEntity<short>
{
    public string? Name { get; set; }

    public virtual ICollection<Extension> Extensions { get; set; } = [];
}

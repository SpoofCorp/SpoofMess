using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofSettingsService.Models;

public partial class GlobalPermission : IdentifiedSoftDeletableEntity<int>
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<User> Users { get; set; } = [];
}

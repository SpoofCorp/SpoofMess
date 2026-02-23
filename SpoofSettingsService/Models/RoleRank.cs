using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofSettingsService.Models;

public partial class RoleRank : IdentifiedEntity<long>
{
    public Guid ChatId { get; set; }

    public short Level { get; set; }

    public string Name { get; set; } = null!;

    public virtual Chat Chat { get; set; } = null!;

    public virtual ICollection<ChatRole> ChatRoles { get; set; } = [];
}

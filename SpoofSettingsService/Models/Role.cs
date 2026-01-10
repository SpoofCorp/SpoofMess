using DataHelpers;

namespace SpoofSettingsService.Models;

public partial class Role : IdentifiedSoftDeletableEntity<int>
{
    public string Name { get; set; } = null!;

    public virtual ICollection<ChatUser> ChatUsers { get; set; } = [];

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
}

using DataHelpers;

namespace SpoofSettingsService.Models;

public partial class Permission : IdentifiedSoftDeletableEntity<short>
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ChatUserPermission> ChatUserPermissions { get; set; } = [];

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
}

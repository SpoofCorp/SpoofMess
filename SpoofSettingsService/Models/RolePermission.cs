using DataHelpers;

namespace SpoofSettingsService.Models;

public partial class RolePermission : SoftDeletableEntity
{
    public int RoleId { get; set; }

    public short PermissionId { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}

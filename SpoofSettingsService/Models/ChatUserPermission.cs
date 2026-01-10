using DataHelpers;

namespace SpoofSettingsService.Models;

public partial class ChatUserPermission : SoftDeletableEntity
{
    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }

    public short PermissionId { get; set; }

    public virtual ChatUser ChatUser { get; set; } = null!;

    public virtual Permission Permission { get; set; } = null!;
}

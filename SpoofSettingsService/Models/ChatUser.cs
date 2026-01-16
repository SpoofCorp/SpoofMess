using DataSaveHelpers;

namespace SpoofSettingsService.Models;

public partial class ChatUser : SoftDeletableEntity
{
    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }

    public int RoleId { get; set; }

    public DateTime JoinedAt { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual ICollection<ChatUserPermission> ChatUserPermissions { get; set; } = [];

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

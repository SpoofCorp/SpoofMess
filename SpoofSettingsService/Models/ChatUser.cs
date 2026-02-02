using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpoofSettingsService.Models;

public partial class ChatUser : DoubleIdentifiedSoftDeletable<Guid, Guid>
{
    [Column("ChatId")]
    public new Guid Key1 { get; set; }

    [Column("UserId")]
    public new Guid Key2 { get; set; }

    public int RoleId { get; set; }

    public DateTime JoinedAt { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual ICollection<ChatUserRule> ChatUserRules { get; set; } = [];

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

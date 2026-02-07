using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpoofSettingsService.Models;

public partial class ChatUser : DoubleIdentifiedSoftDeletable<Guid, Guid>
{
    /// <summary>
    /// ChatId
    /// </summary>
    [Column("ChatId")]
    public new Guid Key1 { get; set; }
    /// <summary>
    /// UserId
    /// </summary>

    [Column("UserId")]
    public new Guid Key2 { get; set; }

    public DateTime JoinedAt { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual ICollection<ChatUserChatRole> ChatUserChatRoles { get; set; } = [];

    public virtual ICollection<ChatUserRule> ChatUserRules { get; set; } = [];

    public virtual User User { get; set; } = null!;
}

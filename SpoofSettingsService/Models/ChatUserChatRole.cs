using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpoofSettingsService.Models;

public partial class ChatUserChatRole : DoubleIdentifiedSoftDeletable<Guid, Guid>
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

    public long ChatRoleId { get; set; }

    public DateTime TimeSet { get; set; }

    public virtual ChatRole ChatRole { get; set; } = null!;

    public virtual ChatUser ChatUser { get; set; } = null!;
}

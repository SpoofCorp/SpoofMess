using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpoofMessageService.Models;

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

    public long Rules { get; set; }

    public DateTime JoinedAt { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

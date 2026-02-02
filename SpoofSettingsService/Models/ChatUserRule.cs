using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpoofSettingsService.Models;

public partial class ChatUserRule : DoubleIdentifiedSoftDeletable<Guid, Guid>
{
    [Column("ChatId")]
    public new Guid Key1 { get; set; }
    [Column("UserId")]
    public new Guid Key2 { get; set; }

    public bool IsPermission { get; set; }

    public short PermissionId { get; set; }

    public virtual ChatUser ChatUser { get; set; } = null!;

    public virtual Permission Permission { get; set; } = null!;
}

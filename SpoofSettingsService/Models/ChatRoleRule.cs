using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpoofSettingsService.Models;

public partial class ChatRoleRule : DoubleIdentifiedEntity<long, short>
{
    /// <summary>
    /// ChatRoleId
    /// </summary>
    [Column("ChatRoleId")]
    public new long Key1 { get; set; }

    /// <summary>
    /// PermissionId
    /// </summary>
    [Column("PermissionId")]
    public new short Key2 { get; set; }

    public bool IsPermission { get; set; }

    public virtual ChatRole ChatRole { get; set; } = null!;

    public virtual Permission Permission { get; set; } = null!;
}

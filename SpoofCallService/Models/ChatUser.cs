using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;

namespace SpoofCallService.Models;

public partial class ChatUser : DoubleIdentifiedSoftDeletableChangeableEntity<Guid, Guid>
{
    /// <summary>
    /// ChatId
    /// </summary>
    public new Guid Key1 { get; set; }

    /// <summary>
    /// UserId
    /// </summary>
    public new Guid Key2 { get; set; }

    public long Rules { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

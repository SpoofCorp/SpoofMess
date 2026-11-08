using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;

namespace SpoofCallService.Models;

public partial class CallMember : DoubleIdentifiedSoftDeletableChangeableEntity<Guid,  Guid>
{

    /// <summary>
    /// CallId
    /// </summary>
    public new Guid Key1 { get; set; }

    /// <summary>
    /// UserId
    /// </summary>
    public new Guid Key2 { get; set; }

    public virtual Call Call { get; set; } = null!;

    public virtual ICollection<CallMemberSession> CallMemberSessions { get; set; } = [];

    public virtual User User { get; set; } = null!;
}

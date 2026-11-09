using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofCallService.Models;

public partial class CallMemberSession : IdentifiedEntity<Guid>
{
    public Guid CallId { get; set; }

    public Guid UserId { get; set; }

    public DateTime JoinedAt { get; set; }

    public DateTime? LeftAt { get; set; }

    public string DeviceId { get; set; } = null!;

    public virtual CallMember CallMember { get; set; } = null!;
}

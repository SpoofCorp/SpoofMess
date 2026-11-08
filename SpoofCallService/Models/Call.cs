using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofCallService.Models;

public partial class Call : IdentifiedSoftDeletableChangeableEntity<Guid>
{
    public Guid ChatId { get; set; }

    public Guid InitiatorId { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? AnsweredAt { get; set; }

    public DateTime? EndedAt { get; set; }

    public string? RoomId { get; set; }

    public virtual ICollection<CallMember> CallMembers { get; set; } = [];

    public virtual Chat Chat { get; set; } = null!;

    public virtual User Initiator { get; set; } = null!;
}

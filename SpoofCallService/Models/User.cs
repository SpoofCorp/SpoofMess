using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofCallService.Models;

public partial class User : IdentifiedSoftDeletableChangeableEntity<Guid>
{
    public string Name { get; set; } = null!;

    public string Login { get; set; } = null!;

    public virtual ICollection<CallMember> CallMembers { get; set; } = [];

    public virtual ICollection<Call> Calls { get; set; } = [];

    public virtual ICollection<ChatUser> ChatUsers { get; set; } = [];

    public virtual ICollection<Chat> Chats { get; set; } = [];
}

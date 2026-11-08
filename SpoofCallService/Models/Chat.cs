using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofCallService.Models;

public partial class Chat : IdentifiedSoftDeletableChangeableEntity<Guid>
{
    public Guid? OwnerId { get; set; }

    public string UniqueName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Call> Calls { get; set; } = [];

    public virtual ICollection<ChatUser> ChatUsers { get; set; } = [];

    public virtual User? Owner { get; set; }
}

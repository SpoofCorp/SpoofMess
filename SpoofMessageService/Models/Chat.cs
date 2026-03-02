using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofMessageService.Models;

public partial class Chat : IdentifiedSoftDeletableChangeableEntity<Guid>
{
    public byte[]? AvatarId { get; set; }

    public string UniqueName { get; set; } = null!;

    public string? Name { get; set; }

    public virtual ICollection<ChatUser> ChatUsers { get; set; } = [];

    public virtual ICollection<Message> Messages { get; set; } = [];
}

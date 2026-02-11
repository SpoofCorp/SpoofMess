using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofMessageService.Models;

public partial class User : IdentifiedSoftDeletableChangeableEntity<Guid>
{
    public Guid? AvatarId { get; set; }

    public string Login { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<ChatUser> ChatUsers { get; set; } = [];

    public virtual ICollection<Message> Messages { get; set; } = [];

    public virtual ICollection<ViewMessage> ViewMessages { get; set; } = [];
}

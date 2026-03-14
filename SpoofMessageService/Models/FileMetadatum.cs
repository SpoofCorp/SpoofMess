using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofMessageService.Models;

public partial class FileMetadatum : IdentifiedSoftDeletableEntity<Guid>
{
    public long Size { get; set; }

    public string Category { get; set; } = null!;

    public virtual ICollection<Attachment> Attachments { get; set; } = [];

    public virtual ICollection<Chat> Chats { get; set; } = [];

    public virtual ICollection<User> Users { get; set; } = [];
}

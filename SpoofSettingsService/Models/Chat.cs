using DataSaveHelpers;

namespace SpoofSettingsService.Models;

public partial class Chat : IdentifiedSoftDeletableEntity<Guid>, IChangeable
{
    public int ChatTypeId { get; set; }

    public Guid? OwnerId { get; set; }

    public string ChatUniqueName { get; set; } = null!;

    public string ChatName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime LastModified { get; set; }

    public virtual ICollection<ChatAvatar> ChatAvatars { get; set; } = new List<ChatAvatar>();

    public virtual ChatType ChatType { get; set; } = null!;

    public virtual ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();

    public virtual User Owner { get; set; } = null!;

    public Chat() { }

    public Chat(int chatTypeId, Guid? ownerId, string chatName, string chatUniqueName, DateTime createdAt, DateTime lastModified)
    {
        ChatTypeId = chatTypeId;
        OwnerId = ownerId;
        ChatName = chatName;
        ChatUniqueName = chatUniqueName;
        CreatedAt = createdAt;
        LastModified = lastModified;
    }
}

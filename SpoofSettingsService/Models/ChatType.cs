using DataHelpers;

namespace SpoofSettingsService.Models;

public partial class ChatType : IdentifiedSoftDeletableEntity<int>
{
    public string Name { get; set; } = null!;

    public virtual ICollection<ChatTypeChatProperty> ChatTypeChatProperties { get; set; } = [];

    public virtual ICollection<Chat> Chats { get; set; } = [];
}

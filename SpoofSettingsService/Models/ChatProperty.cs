using DataSaveHelpers;

namespace SpoofSettingsService.Models;

public partial class ChatProperty : IdentifiedSoftDeletableEntity<short>
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ChatTypeChatProperty> ChatTypeChatProperties { get; set; } = [];
}

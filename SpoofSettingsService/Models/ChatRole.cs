using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofSettingsService.Models;

public partial class ChatRole : IdentifiedSoftDeletableEntity<long>
{
    public Guid ChatId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Chat Chat { get; set; } = null!;

    public virtual ICollection<ChatRoleRule> ChatRoleRules { get; set; } = [];

    public virtual ICollection<ChatUserChatRole> ChatUserChatRoles { get; set; } = [];
}

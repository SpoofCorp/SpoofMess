using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofSettingsService.Models;

public partial class Permission : IdentifiedSoftDeletableEntity<short>
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ChatUserRule> ChatUserRules { get; set; } = [];

    public virtual ICollection<ChatRoleRule> ChatRoleRules { get; set; } = [];
}
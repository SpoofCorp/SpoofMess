using DataSaveHelpers;

namespace SpoofSettingsService.Models;

public partial class ChatTypeChatProperty : SoftDeletableEntity
{
    public int ChatTypeId { get; set; }

    public short ChatPropertyId { get; set; }

    public virtual ChatProperty ChatProperty { get; set; } = null!;

    public virtual ChatType ChatType { get; set; } = null!;
}

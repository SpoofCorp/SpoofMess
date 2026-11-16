using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;

namespace SpoofSettingsService.Models;

public partial class UserStickerPack : DoubleIdentifiedChangeableEntity<Guid, long>
{
    /// <summary>
    /// UserId
    /// </summary>

    public new Guid Key1 { get; set; }

    /// <summary>
    /// StickerPackId
    /// </summary>
    public new long Key2 { get; set; }

    public int Position { get; set; }

    public virtual StickerPack StickerPack { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

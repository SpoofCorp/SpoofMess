using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofSettingsService.Models;

public partial class FileMetadatum : IdentifiedSoftDeletableEntity<Guid>
{
    public long Size { get; set; }

    public short ExtensionId { get; set; }

    public virtual ICollection<ChatAvatar> ChatAvatars { get; set; } = [];

    public virtual Extension Extension { get; set; } = null!;

    public virtual ICollection<FileMetadataOperationStatus> FileMetadataOperationStatuses { get; set; } = [];

    public virtual ICollection<StickerPack> StickerPacks { get; set; } = [];

    public virtual ICollection<Sticker> Stickers { get; set; } = [];

    public virtual ICollection<UserAvatar> UserAvatars { get; set; } = [];
}

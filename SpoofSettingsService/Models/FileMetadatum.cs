using DataSaveHelpers;
using System;
using System.Collections.Generic;

namespace SpoofSettingsService.Models;

public partial class FileMetadatum : IdentifiedSoftDeletableEntity<Guid>
{
    public long Size { get; set; }

    public short ExtensionId { get; set; }

    public virtual Extension Extension { get; set; } = null!;

    public virtual ICollection<FileMetadataOperationStatus> FileMetadataOperationStatuses { get; set; } = new List<FileMetadataOperationStatus>();

    public virtual ICollection<StickerPack> StickerPacks { get; set; } = new List<StickerPack>();
}

using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofMessageService.Models;

public partial class FileMetadatum : IdentifiedSoftDeletableEntity<byte[]>
{
    public long Size { get; set; }

    public short ExtensionId { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = [];

    public virtual Extension Extension { get; set; } = null!;
}

using DataSaveHelpers;

namespace SpoofSettingsService.Models;

public partial class FileMetadataOperationStatus : IdentifiedEntity<long>
{
    public Guid FileMetadataId { get; set; }

    public short OperationStatusId { get; set; }

    public string? Description { get; set; }

    public DateTime TimeSet { get; set; }

    public bool IsActual { get; set; }

    public virtual FileMetadatum FileMetadata { get; set; } = null!;

    public virtual OperationStatus OperationStatus { get; set; } = null!;
}

using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofSettingsService.Models;

public partial class OperationStatus : IdentifiedEntity<short>
{
    public string Name { get; set; } = null!;

    public virtual ICollection<FileMetadataOperationStatus> FileMetadataOperationStatuses { get; set; } = [];
}
namespace SpoofSettingsService.Models;

public partial class OperationStatus
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<FileMetadataOperationStatus> FileMetadataOperationStatuses { get; set; } = [];
}
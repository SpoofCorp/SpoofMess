using DataSaveHelpers.EntityTypesRealizations.Identified;

namespace SpoofMessageService.Models;

public partial class FileType : IdentifiedSoftDeletableEntity<short>
{
    public string Name { get; set; } = null!;
}

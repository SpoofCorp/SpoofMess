using CommonObjects.DTO;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Setters;

public static class FileMatadatumSetter
{
    public static FileMetadata Set(this FileMetadatum fileMetadatum) =>
        new() { Size = fileMetadatum.Size}; 
    
    public static FileMetadatum Set(this FileMetadata fileMetadata) =>
        new() { Id = fileMetadata.Id, Size = fileMetadata.Size };
}
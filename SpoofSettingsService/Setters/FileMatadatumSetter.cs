using CommonObjects.DTO;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Setters;

public static class FileMatadatumSetter
{
    public static FileMetadata Set(this FileMetadatum fileMetadatum) =>
        new() { Name = fileMetadatum.Name, Size = fileMetadatum.Size}; 
    
    public static FileMetadatum Set(this FileMetadata fileMetadata) =>
        new() { Size = fileMetadata.Size, Name = fileMetadata.Name, Id = fileMetadata.Id  };
}
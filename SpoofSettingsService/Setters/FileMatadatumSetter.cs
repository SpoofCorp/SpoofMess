using CommonObjects.DTO;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Setters;

public static class FileMatadatumSetter
{
    public static FileMetadata Set(this FileMetadatum fileMetadatum) =>
        new() { Name = fileMetadatum.Name, Bucket = fileMetadatum.Bucket, ObjectKey = fileMetadatum.ObjectKey, Size = fileMetadatum.Size }; 
    
    public static FileMetadatum Set(this FileMetadata fileMetadata) =>
        new() { Size = fileMetadata.Size, Name = fileMetadata.Name, Bucket = fileMetadata.Bucket, ObjectKey = fileMetadata.ObjectKey };
}
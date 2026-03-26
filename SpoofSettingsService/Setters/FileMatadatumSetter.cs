using CommonObjects.DTO;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Setters;

public static class FileMatadatumSetter
{
    public static FileMetadata Set(this FileMetadatum fileMetadatum, string originalFileName, byte[] token, byte[] id) =>
        new(
            token,
            id, 
            originalFileName,
            fileMetadatum.Size);
    
    public static FileMetadatum Set(this FileMetadata fileMetadata, Guid fileId) =>
        new()
        { 
            Id = fileId, 
            Size = fileMetadata.Size 
        };
}
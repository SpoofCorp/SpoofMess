using CommonObjects.DTO;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Setters;

public static class FileMatadatumSetter
{
    public static FileMetadata Set(this FileMetadatum fileMetadatum) =>
        new() { Name = "", Size = 42 };
}

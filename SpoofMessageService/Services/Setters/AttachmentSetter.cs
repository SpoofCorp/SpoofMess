using CommonObjects.DTO;
using SpoofMessageService.Models;

namespace SpoofMessageService.Services.Setters;

public static class AttachmentSetter
{
    public static Attachment Set(this FileMetadata fileMetadata) =>
        new() { FileMetadata = fileMetadata.SetMetadata() };

    public static FileMetadatum SetMetadata(this FileMetadata fileMetadata) =>
        new() { Id = fileMetadata.Id, Size = fileMetadata.Size };
}

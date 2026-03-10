using CommonObjects.DTO;
using SpoofMessageService.Models;
using SpoofMessageService.Models.Enums;

namespace SpoofMessageService.Services.Setters;

public static class AttachmentSetter
{
    public static Attachment Set(this FileMetadata fileMetadata, OperationsStatus operationsStatus) =>
        new() { FileMetadata = fileMetadata.SetMetadata() };

    public static FileMetadatum SetMetadata(this FileMetadata fileMetadata) =>
        new() { Id = fileMetadata.Id, Size = fileMetadata.Size };

    public static Attachment Set(this CommonObjects.Requests.Attachments.Attachment attachment, Guid fileId) =>
        new() { Key2 = fileId, OriginalFileName = attachment.OriginalFileName };

}

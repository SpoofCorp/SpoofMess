using CommonObjects.DTO;
using SpoofMessageService.Models;

namespace SpoofMessageService.Services.Setters;

public static class AttachmentSetter
{
    public static Attachment Set(this FileMetadata fileMetadata, OperationsStatus operationsStatus) =>
        new() { FileMetadata = fileMetadata.SetMetadata(), AttachmentOperationStatuses = [new() { OperationStatusId = (short)operationsStatus }] };

    public static FileMetadatum SetMetadata(this FileMetadata fileMetadata) =>
        new() { Id = fileMetadata.Id, Size = fileMetadata.Size };
}

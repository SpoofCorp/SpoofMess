using CommonObjects.DTO;
using CommonObjects.Responses;
using SpoofMessageService.Models;
using SpoofMessageService.Models.Enums;

namespace SpoofMessageService.Services.Setters;

public static class AttachmentSetter
{
    public static Attachment Set(this FileMetadata fileMetadata, Guid fileId, OperationsStatus operationsStatus, string? attachmentMetadata) =>
        new()
        {
            FileMetadata = fileMetadata.SetMetadata(fileId),
            AttachmentMetadata = attachmentMetadata,
        };

    public static FileMetadatum SetMetadata(this FileMetadata fileMetadata, Guid fileId) =>
        new()
        {
            Id = fileId,
            Size = fileMetadata.Size
        };

    public static Attachment Set(this CommonObjects.Requests.Attachments.Attachment attachment, Guid fileId, string? attachmentMetadata) =>
        new()
        {
            FileMetadataId = fileId,
            AttachmentMetadata = attachmentMetadata,
            OriginalFileName = attachment.OriginalFileName
        };

    public static Attachment Set(this CommonObjects.Requests.Attachments.Attachment attachment, Guid fileId, FileMetadatum metadatum, string? attachmentMetadata) =>
        new()
        {
            FileMetadataId = fileId,
            FileMetadata = metadatum,
            OriginalFileName = attachment.OriginalFileName,
            Size = metadatum.Size,
            Category = metadatum.Category,
            AttachmentMetadata = attachmentMetadata,
            AdditionalMetadata = metadatum.Metadata,
        };
    public static Attachment Set(this EditAttachment attachment, Guid fileId) =>
    new()
    {
        FileMetadataId = fileId,
        OriginalFileName = attachment.OriginalFileName
    };

    public static Attachment Set(this EditAttachment attachment, Guid fileId, FileMetadatum metadatum) =>
        new()
        {
            FileMetadataId = fileId,
            FileMetadata = metadatum,
            OriginalFileName = attachment.OriginalFileName,
            Size = metadatum.Size,
            Category = metadatum.Category,
            AdditionalMetadata = metadatum.Metadata,
        };
}

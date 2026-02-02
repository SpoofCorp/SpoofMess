using CommonObjects.DTO;

namespace CommonObjects.Requests.Attachments;

public class RemoveAttachmentRequest
{
    public Guid MessageId { get; set; }
    public FileMetadata FileMetadata { get; set; } = null!;
}

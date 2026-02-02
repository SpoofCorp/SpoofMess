using CommonObjects.DTO;

namespace CommonObjects.Requests.Attachments;

public class AddAttachmentRequest
{
    public Guid MessageId { get; set; }
    public FileMetadata FileMetadata { get; set; } = null!;
}

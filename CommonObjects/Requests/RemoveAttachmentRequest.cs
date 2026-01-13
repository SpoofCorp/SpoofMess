using CommonObjects.DTO;

namespace CommonObjects.Requests;

public class RemoveAttachmentRequest
{
    public Guid MessageId { get; set; }
    public FileMetadata FileMetadata { get; set; } = null!;
}

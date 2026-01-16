using CommonObjects.DTO;

namespace CommonObjects.Requests;

public class CreateMessageRequest
{
    public string Text { get; set; } = null!;
    public ICollection<FileMetadata> Attachments { get; set; } = [];
    public Guid ChatId { get; set; }
}

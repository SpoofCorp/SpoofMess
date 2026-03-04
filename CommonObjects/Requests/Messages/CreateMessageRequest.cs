using CommonObjects.Requests.Attachments;

namespace CommonObjects.Requests.Messages;

public class CreateMessageRequest
{
    public string Text { get; set; } = null!;
    public ICollection<Attachment> Attachments { get; set; } = [];
    public Guid ChatId { get; set; }
}

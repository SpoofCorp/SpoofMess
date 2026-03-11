using CommonObjects.Requests.Attachments;

namespace CommonObjects.Requests.Messages;

public class EditMessageRequest
{
    public Guid Id { get; set; }
    public string? Text { get; set; }
    public ICollection<Attachment>? Attachments { get; set; }
    public Guid ChatId { get; set; }
}

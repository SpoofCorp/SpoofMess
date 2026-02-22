namespace CommonObjects.Requests.Messages;

public class DeleteMessageRequest
{
    public Guid Id { get; set; }
    public Guid ChatId { get; set; }
}

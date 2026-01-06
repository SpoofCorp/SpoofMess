namespace CommonObjects.Requests;

public class RemoveChatAvatarRequest
{
    public required Guid ChatId { get; init; }

    public required Guid FileId { get; init; }
}

using CommonObjects.DTO;
namespace CommonObjects.Requests;

public class SetChatAvatarRequest
{
    public required Guid ChatId { get; init; }

    public required Guid FileId { get; init; }

    public required FileMetadata Metadata { get; init; }
}

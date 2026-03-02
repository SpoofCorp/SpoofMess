using CommonObjects.DTO;

namespace CommonObjects.Requests.Avatars;

public class SetChatAvatarRequest
{
    public required Guid ChatId { get; init; }

    public required byte[] FileId { get; init; }

    public required FileMetadata Metadata { get; init; }
}

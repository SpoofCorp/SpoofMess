using CommonObjects.DTO;

namespace CommonObjects.Requests.Avatars;

public class SesUserAvatarRequest
{
    public required Guid UserId { get; init; }

    public required Guid FileId { get; init; }

    public required FileMetadata Metadata { get; init; }
}

namespace CommonObjects.Requests.Avatars;

public class RemoveUserAvatarRequest
{
    public required Guid UserId { get; init; }

    public required Guid FileId { get; init; }
}

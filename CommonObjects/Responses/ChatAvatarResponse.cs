using CommonObjects.DTO;

namespace CommonObjects.Responses;

public class AvatarResponse
{
    public required Guid FileId { get; init; }

    public required FileMetadata FileMetadata { get; set; }
}
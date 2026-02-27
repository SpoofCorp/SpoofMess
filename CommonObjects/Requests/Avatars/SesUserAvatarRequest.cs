using CommonObjects.DTO;

namespace CommonObjects.Requests.Avatars;

public record SesUserAvatarRequest(
        FileMetadata Metadata
    );
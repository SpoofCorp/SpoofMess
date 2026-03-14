using CommonObjects.Requests.Attachments;

namespace CommonObjects.DTO;

public record MessageDTO(
        Guid Id,
        Guid ChatId,
        string SenderLogin,
        string SenderName,
        byte[]? UserAvatarId,
        string Text,
        DateTime SendAt,
        List<Attachment> Attachments
    );

namespace SpoofMessageService;

public readonly record struct IntermediateMessage(
        Guid Id,
        Guid ChatId,
        string SenderLogin,
        string SenderName,
        Guid? SenderAvatar,
        string Text,
        DateTime SendAt,
        MessageAttachment[] Attachments
    );
public readonly record struct MessageAttachment(
        string OriginalFileName,
        long FileSize,
        Guid Id
    );
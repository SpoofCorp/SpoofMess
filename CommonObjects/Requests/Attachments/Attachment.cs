namespace CommonObjects.Requests.Attachments;

public record Attachment(
        byte[] Token,
        string OriginalFileName,
        long Size
    );

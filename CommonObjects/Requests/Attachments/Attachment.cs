namespace CommonObjects.Requests.Attachments;

public record Attachment(
        byte[] Fingerprint,
        string OriginalFileName,
        long Size
    );

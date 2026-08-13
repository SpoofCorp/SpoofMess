namespace SecurityLibrary.Tokens;

public interface IAttachmentAccessTokenService
{
    public bool IsValid(
        ReadOnlySpan<byte> token,
        out Guid attachmentId);

    public byte[] CreateToken(
        Guid attachmentId);
}
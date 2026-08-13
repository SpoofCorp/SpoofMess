namespace SecurityLibrary.Tokens;

public interface IFileTokenService
{
    public bool IsValid(
        ReadOnlySpan<byte> token,
        Guid userId,
        out Guid fileId);

    public byte[] CreateToken(
        Guid userId, 
        Guid fileId);
}
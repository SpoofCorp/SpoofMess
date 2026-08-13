using SecurityLibrary.Tokens;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SecurityLibrary.TokensRealizations;

public class AttachmentAccessTokenService(TokenHeaderCover validToken) : TokenService(validToken), IAttachmentAccessTokenService
{
    protected override int BodySize { get; init; } = Unsafe.SizeOf<AttachmentAccess>();

    public bool IsValid(ReadOnlySpan<byte> token, out Guid attachmentId)
    {
        int bodySize = token.Length - (28 + headerSize);
        if (bodySize < BodySize)
        {
            attachmentId = default;
            return false;
        }

        Span<byte> destination = stackalloc byte[12 + 16 + headerSize + BodySize];
        if (Decrypt(token, destination))
        {
            AttachmentAccess access = MemoryMarshal.Read<AttachmentAccess>(destination);
            attachmentId = access.AttachmentId;
            return true;
        }
        attachmentId = default;
        return false;
    }

    public byte[] CreateToken(Guid attachmentId)
    {
        AttachmentAccess access = new() { AttachmentId = attachmentId };

        ReadOnlySpan<byte> body = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref access, 1));
        byte[] destination = new byte[12 + 16 + headerSize + BodySize];
        Encrypt(body, destination);
        return destination;
    }
}
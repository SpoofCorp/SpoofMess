using SecurityLibrary.Tokens;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SecurityLibrary.TokensRealizations;

public class FileTokenService(LivedTokenHeaderCover validToken) : LivedTokenService(validToken), IFileTokenService
{
    protected override int BodySize { get; init; } = Unsafe.SizeOf<Tokens.FileAccess>();

    public bool IsValid(ReadOnlySpan<byte> token, Guid userId, out Guid fileId)
    {
        int bodySize = token.Length - (28 + headerSize);
        if (bodySize < BodySize)
        {
            fileId = default;
            return false;
        }

        Span<byte> destination = stackalloc byte[12 + 16 + headerSize + BodySize];
        ReadOnlySpan<byte> userBytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref userId, 1));
        if (Decrypt(token, destination, userBytes))
        {
            Tokens.FileAccess access = MemoryMarshal.Read<Tokens.FileAccess>(destination);
            GetMask(access.Mask, userBytes, out fileId);
            return true;
        }
        fileId = default;
        return false;
    }

    public byte[] CreateToken(Guid userId, Guid fileId)
    {
        Tokens.FileAccess access = new();
        ReadOnlySpan<byte> userBytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref userId, 1));
        GetMask(fileId, userBytes, out access.Mask);

        ReadOnlySpan<byte> body = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref access, 1));
        byte[] destination = new byte[12 + 16 + headerSize + BodySize];
        Encrypt(body, destination, userBytes);
        return destination;
    }

    private static void GetMask(Guid secondId, ReadOnlySpan<byte> userBytes, out Guid mask)
    {
        Span<byte> fileBytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref secondId, 1));
        for (int i = 0; i < fileBytes.Length; i++)
            fileBytes[i] ^= userBytes[i];
        MemoryMarshal.TryRead(fileBytes, out mask);
    }
}
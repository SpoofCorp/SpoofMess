using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace SecurityLibrary.Tokens;

public abstract class LivedTokenService : IDisposable
{
    private readonly byte[] secretKey = new byte[32];
    protected readonly int headerSize = Unsafe.SizeOf<LivedTokenHeader>();
    protected abstract int BodySize { get; init; }

    private readonly AesGcm aesGcm;
    protected LivedTokenHeader _validHeader;
    public LivedTokenService(LivedTokenHeaderCover validHeader)
    {
        //RandomNumberGenerator.Fill(secretKey);
        secretKey = [36, 21, 247, 223, 154, 202, 63, 250, 36, 59, 84, 55, 126, 134, 184, 209, 127, 91, 80, 14, 55, 54, 164, 96, 183, 143, 95, 230, 38, 20, 87, 203];
        aesGcm = new(secretKey, tagSizeInBytes: 16);
        _validHeader = validHeader.Header;
    }

    protected int Encrypt(ReadOnlySpan<byte> body, Span<byte> destination, ReadOnlySpan<byte> salt = default)
    {
        int tokenSize = 12 + 16 + headerSize + body.Length;
        if (destination.Length < tokenSize)
            return -1;
        LivedTokenHeader header = _validHeader with { ExpiresAt = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds(), BodySize = body.Length };

        Span<byte> nonce = destination[..12];
        Span<byte> tagDest = destination.Slice(12, 16);
        Span<byte> headerDest = destination.Slice(28, headerSize);
        Span<byte> ciphertext = destination[(28 + headerSize)..];

        RandomNumberGenerator.Fill(nonce);
        MemoryMarshal.Write(headerDest[..headerSize], in header);
        Span<byte> authData = stackalloc byte[headerSize + salt.Length];
        headerDest.CopyTo(authData[..headerSize]);
        if (!salt.IsEmpty)
            salt.CopyTo(authData[headerSize..]);

        aesGcm.Encrypt(nonce, body, ciphertext, tagDest, authData);
        return tokenSize;
    }

    protected bool Decrypt(ReadOnlySpan<byte> token, Span<byte> destination, ReadOnlySpan<byte> salt = default)
    {
        if (token.Length < 12 + 16 + headerSize)
            return false;

        if (destination.Length < token.Length - (28 + headerSize))
            return false;

        ReadOnlySpan<byte> nonce = token[..12];
        ReadOnlySpan<byte> tag = token.Slice(12, 16);
        ReadOnlySpan<byte> headerBytes = token.Slice(28, headerSize);
        ReadOnlySpan<byte> ciphertext = token[(28 + headerSize)..];

        Span<byte> authData = stackalloc byte[headerSize + salt.Length];
        headerBytes.CopyTo(authData[..headerSize]);
        if (!salt.IsEmpty)
            salt.CopyTo(authData[headerSize..]);

        try
        {
            aesGcm.Decrypt(nonce, ciphertext, tag, destination[..ciphertext.Length], authData);
            LivedTokenHeader header = MemoryMarshal.Read<LivedTokenHeader>(headerBytes);

            return header.Version == _validHeader.Version
                && header.Magic == _validHeader.Magic
                && header.BodySize == BodySize
                && header.ExpiresAt > DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        aesGcm?.Dispose();
        GC.SuppressFinalize(this);
    }
}
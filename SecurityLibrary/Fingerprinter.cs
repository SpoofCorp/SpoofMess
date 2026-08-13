using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace SecurityLibrary;

public static class Fingerprinter
{
    public static async Task<byte[]> GetFingerPrint(string filePath)
    {
        using FileStream stream = File.OpenRead(filePath);
        return await SHA256.HashDataAsync(stream);
    }
    public static async Task<FileResult> GetFingerPrint(IFormFile file)
    {
        return await GetFingerPrint(file.OpenReadStream());
    }

    public static async Task<FileResult> GetFingerPrint(Stream stream)
    {
        string filePath = Path.GetTempFileName();
        byte[] hash;
        using FileStream fileStream = File.Create(filePath);
        using SHA256 sha256 = SHA256.Create();
        using CryptoStream cryptoStream = new(fileStream, sha256, CryptoStreamMode.Write);
        await stream.CopyToAsync(cryptoStream);
        cryptoStream.FlushFinalBlock();
        hash = sha256.Hash;

        return new(hash, filePath, stream.Length);
    }

    public static async Task<FingerprintFull> GetFull(Stream stream)
    {
        byte[] hash;
        string filePath = Path.GetTempFileName();
        using FileStream fileStream = File.Create(filePath);
        using SHA256 sha256 = SHA256.Create();
        using CryptoStream cryptoStream = new(fileStream, sha256, CryptoStreamMode.Write);
        
        await stream.CopyToAsync(cryptoStream);
        cryptoStream.FlushFinalBlock();
        hash = sha256.Hash!;

        return new(
                fileStream.Length >= 50 * 1024 * 1024 ? await GetFingerPrintL1(fileStream) : null,
                fileStream.Length >= 50 * 1024 * 1024 ? await GetFingerPrintL2(fileStream) : null,
                new(hash!, filePath, fileStream.Length)
            );
    }

    public static async Task<byte[]> GetFingerPrintL1(string filePath)
    {
        return await GetFingerPrintL1(File.OpenRead(filePath));
    }

    public static async Task<byte[]> GetFingerPrintL2(string filePath)
    {
        return await GetFingerPrintL2(File.OpenRead(filePath));
    }

    public static async Task<byte[]> GetFingerPrintL1(FileStream stream)
    {
        byte[] buffer = new byte[64 * 1024];
        stream.Position = 0;
        await stream.ReadExactlyAsync(buffer, 0, buffer.Length);

        return SHA256.HashData(buffer);
    }

    public static async Task<byte[]> GetFingerPrintL2(FileStream stream)
    {
        int chunkSize = 64 * 1024;
        byte[] buffer = new byte[chunkSize * 3];
        stream.Position = 0;
        await stream.ReadExactlyAsync(buffer, 0, chunkSize);
        stream.Position = stream.Length / 2;
        await stream.ReadExactlyAsync(buffer, chunkSize, chunkSize);
        stream.Position = stream.Length - chunkSize;
        await stream.ReadExactlyAsync(buffer, chunkSize * 2, chunkSize);
        return SHA256.HashData(buffer);
    }
}
public record struct FileResult(
        byte[] Fingerprint,
        string FilePath,
        long Size
    );
public record struct FingerprintFull(
        byte[]? L1,
        byte[]? L2,
        FileResult FileResult
    );

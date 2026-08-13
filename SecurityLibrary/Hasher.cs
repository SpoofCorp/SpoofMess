using System.Security.Cryptography;
using System.Text;
using System.IO.Hashing;

namespace SecurityLibrary;

public class Hasher(HashSettings settings)
{
    private readonly HashSettings settings = settings;

    /// <summary>
    /// Hash a password
    /// </summary>
    /// <param name="password">The password to hash</param>
    /// <returns>The hashed password</returns>
    /// <exception cref="ArgumentException">Throw when password is null or empty</exception>
    public string HashPassword(string password) =>
        HashPassword(password, 11);

    /// <summary>
    /// Hash a password
    /// </summary>
    /// <param name="password">The password to hash</param>
    /// <param name="workFactor">2^workFactor of rounds of hashing</param>
    /// <returns>The hashed password</returns>
    /// <exception cref="ArgumentException">Throw when password is null or empty</exception>
    /// <exception cref="ArgumentOutOfRangeException">Throw when workFactor not in between 4 and 30</exception>
    public string HashPassword(string password, int workFactor)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Data can not be null or empty", nameof(password));

        if (workFactor < 4 || workFactor > 30)
            throw new ArgumentOutOfRangeException(nameof(workFactor), "A value of workFactor between 4 and 30 is required");

        string processedPassword = HashKey(password + settings.Paper);


        return BCrypt.Net.BCrypt.HashPassword(processedPassword, workFactor: workFactor);
    }

    /// <summary>
    /// Verify password by hash
    /// </summary>
    /// <param name="password">The password to verify</param>
    /// <param name="hash">The previosly hashed password</param>
    /// <returns>True if the password match, false otherwise</returns>
    /// <exception cref="ArgumentException">Throw when password or hash is null or empty</exception>
    public bool VerifyPassword(string password, string hash)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password can not be null or empty", nameof(password));

        if (string.IsNullOrEmpty(hash))
            throw new ArgumentException("Hash can not be null or empty", nameof(hash));

        string processedPassword = HashKey(password + settings.Paper);

        return BCrypt.Net.BCrypt.Verify(processedPassword, hash);
    }

    public byte[] GetKey(byte[] data)
    {
        return HMACSHA256.HashData(settings.HashKey, data)[..16];
    }

    /// <summary>
    /// Hash a key
    /// </summary>
    /// <param name="key">The key to hash</param>
    /// <returns>The hashed key</returns>
    /// <exception cref="ArgumentException">Throw when key is null or empty</exception>
    public static string HashKey(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Data can not be null or empty", nameof(key));

        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        return Convert.ToBase64String(hash);
    }


    public static async Task<byte[]> Fingerprint(Stream stream)
    {
        XxHash3 hasher = new();
        byte[] buffer = new byte[128 * 1024];
        int bytesRead;
        while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
        {
            hasher.Append(buffer.AsSpan(0, bytesRead));
        }
        return hasher.GetCurrentHash();
    }
}
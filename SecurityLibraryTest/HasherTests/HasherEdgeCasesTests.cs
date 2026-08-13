using System.Text;
using SecurityLibrary;

namespace SecurityLibraryTest.HasherTests;

public class HasherEdgeCasesTests
{
    private readonly Hasher hasher = new(new());

    [Fact]
    public void Hash_and_verify_long_password()
    {
        string arrange = new('f', 1000);

        string hash = hasher.HashPassword(arrange, 4);

        Assert.True(hasher.VerifyPassword(arrange, hash));
    }

    [Fact]
    public void Hash_and_verify_long_key()
    {
        string arrange = new('f', 1000);

        string hash1 = Hasher.HashKey(arrange);
        string hash2 = Hasher.HashKey(arrange);

        Assert.Equal(hash1, hash2);
    }
    [Fact]
    public void Hash_and_verify_uncorrect_long_password_with_extra_char()
    {
        string arrange = new('f', 1000);

        string hash = hasher.HashPassword(arrange, 4);

        Assert.False(hasher.VerifyPassword(arrange + "f", hash));
    }

    [Fact]
    public void Hash_and_verify_uncorrect_long_key_with_extra_char()
    {
        string arrange = new('f', 1000);

        string hash1 = Hasher.HashKey(arrange);
        string hash2 = Hasher.HashKey(arrange + "f");

        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash_and_verify_exactly_72_bytes_password()
    {
        string arrange = new('a', 72);

        Assert.Equal(72, Encoding.UTF8.GetByteCount(arrange));

        string hash = hasher.HashPassword(arrange, 4);
        Assert.True(hasher.VerifyPassword(arrange, hash));
    }
}

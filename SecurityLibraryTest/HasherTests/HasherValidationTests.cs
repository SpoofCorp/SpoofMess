using SecurityLibrary;

namespace SecurityLibraryTest.HasherTests;

public class HasherValidationTests
{
    private readonly Hasher hasher = new(new());
    [Fact]
    public void Null_password_throws()
    {
        Assert.Throws<ArgumentException>(() => hasher.HashPassword(null!, 4));
    }

    [Fact]
    public void Empty_password_throws()
    {
        Assert.Throws<ArgumentException>(() => hasher.HashPassword("", 4));
    }

    [Fact]
    public void Verify_password_empty_hash_throws()
    {
        Assert.Throws<ArgumentException>(() => hasher.VerifyPassword("test", ""));
    }

    [Fact]
    public void Verify_password_null_hash_throws()
    {
        Assert.Throws<ArgumentException>(() => hasher.VerifyPassword("test", null!));
    }

    [Fact]
    public void Verify_password_null_password_throws()
    {
        Assert.Throws<ArgumentException>(() => hasher.VerifyPassword(null!, "test"));
    }

    [Fact]
    public void Verify_password_empty_password_throws()
    {
        Assert.Throws<ArgumentException>(() => hasher.VerifyPassword("", "test"));
    }

}

using SecurityLibrary;

namespace SecurityLibraryTest.HasherTests;

public class HasherTests
{
    private readonly Hasher hasher = new(new());

    [Theory]
    [InlineData("12345678")]
    [InlineData("😂🤡💀🐱‍👤🙈🙉🙊🐵")]
    [InlineData(",.?!/\\:;'\"_-+=&^%$#@*()`~")]
    [InlineData("okak 42!")]
    public void Hash_and_verify_password(string password)
    {
        string hash = hasher.HashPassword(password, 4);

        Assert.False(string.IsNullOrEmpty(hash));

        Assert.True(hasher.VerifyPassword(password, hash));
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("😂🤡💀🐱‍👤🙈🙉🙊🐵")]
    [InlineData(",.?!/\\:;'\"_-+=&^%$#@*()`~")]
    [InlineData("okak 42!")]
    public void Hash_and_verify_key(string key)
    {
        string hash1 = Hasher.HashKey(key);
        string hash2 = Hasher.HashKey(key);

        Assert.Equal(hash1, hash2);
    }

    [Theory]
    [InlineData("12345678", 4, 8)]
    [InlineData("😂🤡💀🐱‍👤🙈🙉🙊🐵", 11, 13)]
    [InlineData(",.?!/\\:;'\"_-+=&^%$#@*()`~", 7, 9)]
    [InlineData("okak 42!", 6, 12)]
    public void Hash_and_verify_password_with_different_work_factor(string password, int workFactor1, int workFactor2)
    {
        string hash1 = hasher.HashPassword(password, workFactor1);
        string hash2 = hasher.HashPassword(password, workFactor2);

        Assert.False(string.IsNullOrEmpty(hash1));
        Assert.False(string.IsNullOrEmpty(hash2));

        Assert.NotEqual(hash1, hash2);
    }

    [Theory]
    [InlineData("12345678", "425262")]
    [InlineData("😂🤡💀🐱‍👤🙈🙉🙊🐵", "✖✖✖😥😥😥🎇")]
    [InlineData(",.?!/\\:;'\"_-+=&^%$#@*()`~", "\\\\!//")]
    [InlineData("okak 42!", "otak 52?")]
    public void Hash_and_verify_different_passwords(string password1, string password2)
    {
        string hash1 = hasher.HashPassword(password1, 4);
        string hash2 = hasher.HashPassword(password2, 4);

        Assert.False(string.IsNullOrEmpty(hash1));
        Assert.False(string.IsNullOrEmpty(hash2));

        Assert.NotEqual(hash1, hash2);
    }
}

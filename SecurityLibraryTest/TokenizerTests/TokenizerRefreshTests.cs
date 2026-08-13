using SecurityLibrary;

namespace SecurityLibraryTest.TokenizerTests;

public class TokenizerRefreshTests
{
    [Fact]
    public void Create_refresh_not_empty_or_null()
    {
        (string token, string tokenHash) = Tokenizer.CreateRefresh();

        Assert.False(string.IsNullOrEmpty(token));
        Assert.False(string.IsNullOrEmpty(tokenHash));
        Assert.NotEqual(token, tokenHash);
    }

    [Fact]
    public void Create_refresh_hash_match_token()
    {
        (string token, string tokenHash) = Tokenizer.CreateRefresh();
        string hash = Hasher.HashKey(token);

        Assert.Equal(hash, tokenHash);
    }
}

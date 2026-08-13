using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SecurityLibrary;

namespace SecurityLibraryTest.TokenizerTests;

public class TokenizerAccessTests
{
    readonly Guid _userId = Guid.CreateVersion7();
    readonly Guid _sessionId = Guid.CreateVersion7();
    readonly string _login = "testLogin";

    [Fact]
    public void Create_access_returns_correct_format()
    {
        string access = Tokenizer.CreateAccess(_login, _userId, _sessionId);

        Assert.False(string.IsNullOrEmpty(access));
        Assert.Equal(2, access.Where(x => x == '.').Count());
    }

    [Fact]
    public void Create_access_token_contains_correct_claims()
    {
        string token = Tokenizer.CreateAccess(_login, _userId, _sessionId);

        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

        Assert.Equal(_userId.ToString(), jwtToken.Claims.First(c => c.Type == "UserId").Value);
        Assert.Equal(_login, jwtToken.Claims.First(c => c.Type == "Login").Value);
        Assert.Equal(_sessionId.ToString(), jwtToken.Claims.First(c => c.Type == "SessionId").Value);
    }

    [Fact]
    public void Create_access_token_has_correct_issuer_and_audience()
    {
        string token = Tokenizer.CreateAccess(_login, _userId, _sessionId);

        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

        Assert.Equal(AuthOptions.ISSUER, jwtToken.Issuer);
        Assert.Contains(AuthOptions.AUDIENCE, jwtToken.Audiences);
    }

    [Fact]
    public void Create_access_with_valid_signature()
    {
        string token = Tokenizer.CreateAccess(_login, _userId, _sessionId);

        JwtSecurityTokenHandler handler = new();
        TokenValidationParameters parameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSecurityKey(),
        };

        ClaimsPrincipal? principal = handler.ValidateToken(token, parameters, out _);

        Assert.NotNull(principal);
        Assert.True(principal.Identity?.IsAuthenticated);
    }

    [Fact]
    public void Create_access_check_life_time()
    {
        string token = Tokenizer.CreateAccess(_login, _userId, _sessionId);
        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken access = handler.ReadJwtToken(token);

        Assert.True(access.ValidFrom < DateTime.UtcNow);

        Assert.True(access.ValidTo >= DateTime.UtcNow);
    }
}

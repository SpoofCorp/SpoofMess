using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace SecurityLibrary;

public class Tokenizer
{
    public static (string Token, string TokenHash) CreateRefresh()
    {
        string token = Guid.CreateVersion7().ToString();
        return (token, Hasher.HashKey(token));
    }

    public static string CreateAccess(string login, Guid userId, Guid sessionId)
    {
        DateTime now = DateTime.UtcNow;
        List<Claim> claims = [new("UserId", userId.ToString()), new("Login", login), new("SessionId", sessionId.ToString())];

        JwtSecurityToken jwt = new(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType).Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}

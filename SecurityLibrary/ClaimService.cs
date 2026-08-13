using System.Security.Claims;

namespace SecurityLibrary;

public class ClaimService
{
    public static Guid GetSessionId(ClaimsPrincipal? user) => GetClaim(user, "SessionId");
    public static Guid GetUserId(ClaimsPrincipal? user) => GetClaim(user, "UserId");

    private static Guid GetClaim(ClaimsPrincipal? user, string claimName)
    {
        ArgumentNullException.ThrowIfNull(user);

        string claim = user.FindFirst(claimName)?.Value 
            ?? throw new ArgumentNullException($"{claimName} doesn't exist in claims");

        if (!Guid.TryParse(claim, out Guid id))
            throw new FormatException($"{claimName}:{claim} not match Guid format");

        return id;
    }
}

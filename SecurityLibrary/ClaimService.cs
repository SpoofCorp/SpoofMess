using System.Security.Claims;

namespace SecurityLibrary;

public class ClaimService
{
    /// <summary>
    ///     Retrieves UserId and SessionId
    /// </summary>
    /// <param name="user"></param>
    /// <returns>
    ///     The user and session ids
    /// </returns>
    public static (Guid SessionId, Guid UserId) GetUserInfo(ClaimsPrincipal? user) => (GetSessionId(user), GetUserId(user));

    /// <summary>
    ///     Retrieves SessionId
    /// </summary>
    /// <param name="user"></param>
    /// <returns>
    ///     The SessionId
    /// </returns>
    public static Guid GetSessionId(ClaimsPrincipal? user) => GetClaim(user, "SessionId");


    /// <summary>
    ///     Retrieves UserId
    /// </summary>
    /// <param name="user"></param>
    /// <returns>
    ///     The UserId
    /// </returns>
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

using CommonObjects.Results;
using SpoofEntranceService.Models;
using SpoofEntranceService.ServiceRealizations.Validators;

namespace SpoofEntranceServiceTest.ValidatorsTests;

public class TokenValidatorTests
{
    [Theory]
    [MemberData(nameof(GetTokens))]
    public void Check_validate_token(Token token, int statusCode)
    {
        TokenValidator sut = new();

        Result result = sut.ValidateToken(token);

        Assert.Equal(statusCode, result.StatusCode);
    }

    public static IEnumerable<object[]> GetTokens()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new Token { IsDeleted = true}, 401 };
        yield return new object[] { new Token { IsDeleted = false, ValidTo = DateTime.UtcNow.AddSeconds(-1)}, 400 };
        yield return new object[] { new Token { IsDeleted = false, ValidTo = DateTime.UtcNow.AddSeconds(1)}, 200 };
    }

}

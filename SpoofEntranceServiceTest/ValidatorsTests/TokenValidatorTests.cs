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

    public static TheoryData<Token, int> GetTokens()
    {
        TheoryData<Token, int> data = [];
        data.Add(null!, 404);
        data.Add(new Token { IsDeleted = true }, 401);
        data.Add(new Token { IsDeleted = false, ValidTo = DateTime.UtcNow.AddSeconds(-1) }, 400);
        data.Add(new Token { IsDeleted = false, ValidTo = DateTime.UtcNow.AddSeconds(1) }, 200);
        return data;
    }

}

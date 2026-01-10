using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.ServiceRealizations.Validators;

namespace SpoofSettingsServiceTests.ValidatorsTests;

public class UserValidatorTests
{
    [Theory]
    [MemberData(nameof(GetUsers))]
    public void Validate_user_to_null_or_deleted(User entity, int statusCode)
    {
        UserValidator sut = new();

        Result result = sut.IsAvailable(entity);

        Assert.Equal(statusCode, result.StatusCode);
    }

    public static IEnumerable<object[]> GetUsers()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new User { IsDeleted = true }, 400 };
        yield return new object[] { new User { IsDeleted = false }, 200 };
    }
}
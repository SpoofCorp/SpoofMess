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

    public static TheoryData<User, int> GetUsers()
    {
        TheoryData<User, int> data = [];
        data.Add(null!, 404);
        data.Add(new User { IsDeleted = true }, 400);
        data.Add(new User { IsDeleted = false }, 200);
        return data;
    }
}
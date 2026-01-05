using CommonObjects.Results;
using SpoofEntranceService.Models;
using SpoofEntranceService.ServiceRealizations.Validators;

namespace SpoofEntranceServiceTest.ValidatorsTests;

public class UserEntryValidatorTests
{
    [Theory]
    [MemberData(nameof(GetUserEntries))]
    public void Check_validate_user_entry(UserEntry user, int statusCode)
    {
        UserEntryValidator sut = new();

        Result result = sut.IsActive(user);

        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetActiveUserEntries))]
    public void Check_active_user_entry(UserEntry user, int statusCode)
    {
        UserEntryValidator sut = new();

        Result result = sut.HisIsActive(user);

        Assert.Equal(statusCode, result.StatusCode);
    }

    public static IEnumerable<object[]> GetUserEntries()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new UserEntry { IsDeleted = true }, 400 };
        yield return new object[] { new UserEntry { IsDeleted = false }, 200 };
    }

    public static IEnumerable<object[]> GetActiveUserEntries()
    {
        yield return new object[] { null!, 200 };
        yield return new object[] { new UserEntry { IsDeleted = true }, 200 };
        yield return new object[] { new UserEntry { IsDeleted = false }, 400 };
    }
}

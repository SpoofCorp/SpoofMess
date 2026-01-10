using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.ServiceRealizations.Validators;

namespace SpoofSettingsServiceTests.ValidatorsTests;

public class UserAvatarValidatorTests
{
    [Theory]
    [MemberData(nameof(GetAvatars))]
    public void Validate_awatars_collection(List<UserAvatar> entities, int statusCode)
    {
        UserAvatarValidator sut = new();

        Result result = sut.IsAvailableCollection(entities);

        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetAvatarWithFile))]
    public void Validate_file_to_null_deleted_and_file(UserAvatar entity, int statusCode)
    {
        UserAvatarValidator sut = new();

        Result result = sut.FileIsActive(entity);

        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetAvatar))]
    public void Validate_file_to_null_deleted(UserAvatar entity, int statusCode)
    {
        UserAvatarValidator sut = new();

        Result result = sut.IsAvailable(entity);

        Assert.Equal(statusCode, result.StatusCode);
    }

    public static IEnumerable<object[]> GetAvatars()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new List<UserAvatar>(), 400 };
        yield return new object[] { new List<UserAvatar> { new() }, 200 };
        yield return new object[] { new List<UserAvatar> { new(), new(), new() }, 200 };
    }
    public static IEnumerable<object[]> GetAvatarWithFile()

    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new UserAvatar { IsDeleted = true, FileId = null }, 400 };
        yield return new object[] { new UserAvatar { IsDeleted = true, FileId = new() }, 400 };
        yield return new object[] { new UserAvatar { IsDeleted = false, FileId = null }, 400 };
        yield return new object[] { new UserAvatar { IsDeleted = false, FileId = new() }, 200 };
    }
    public static IEnumerable<object[]> GetAvatar()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new UserAvatar { IsDeleted = true }, 400 };
        yield return new object[] { new UserAvatar { IsDeleted = false }, 200 };
    }
}

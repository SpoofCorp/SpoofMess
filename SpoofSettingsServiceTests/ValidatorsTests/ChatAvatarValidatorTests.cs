using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.ServiceRealizations.Validators;

namespace SpoofSettingsServiceTests.ValidatorsTests;

public class ChatAvatarValidatorTests
{
    [Theory]
    [MemberData(nameof(GetAvatars))]
    public void Validate_awatars_collection(List<ChatAvatar> entities, int statusCode)
    {
        ChatAvatarValidator sut = new();

        Result result = sut.IsAvailableCollection(entities);

        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetAvatarWithFile))]
    public void Validate_file_to_null_deleted_and_file(ChatAvatar entity, int statusCode)
    {
        ChatAvatarValidator sut = new();

        Result result = sut.FileIsActive(entity);

        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetAvatar))]
    public void Validate_file_to_null_deleted(ChatAvatar entity, int statusCode)
    {
        ChatAvatarValidator sut = new();

        Result result = sut.IsAvailable(entity);

        Assert.Equal(statusCode, result.StatusCode);
    }

    public static IEnumerable<object[]> GetAvatars()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new List<ChatAvatar>(), 400 };
        yield return new object[] { new List<ChatAvatar> { new() }, 200 };
        yield return new object[] { new List<ChatAvatar> { new(), new(), new() }, 200 };
    }
    public static IEnumerable<object[]> GetAvatarWithFile()

    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new ChatAvatar { IsDeleted = true, FileId = null }, 400 };
        yield return new object[] { new ChatAvatar { IsDeleted = true, FileId = new() }, 400 };
        yield return new object[] { new ChatAvatar { IsDeleted = false, FileId = null }, 400 };
        yield return new object[] { new ChatAvatar { IsDeleted = false, FileId = new() }, 200 };
    }
    public static IEnumerable<object[]> GetAvatar()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new ChatAvatar { IsDeleted = true }, 400 };
        yield return new object[] { new ChatAvatar { IsDeleted = false }, 200 };
    }
}

using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.ServiceRealizations.Validators;

namespace SpoofSettingsServiceTests.ValidatorsTests;

public class ChatValidatorTests 
{
    [Theory]
    [MemberData(nameof(GetChat))]
    public void Validate_chat_to_null_or_deleted(Chat entity, int statusCode)
    {
        ChatValidator sut = new();

        Result result = sut.IsAvailable(entity);

        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetChatType))]
    public void Validate_chatType_to_null_or_deleted(ChatType entity, int statusCode)
    {
        ChatValidator sut = new();

        Result result = sut.ValidateChatType(entity);

        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetChatForUniqueName))]
    public void Validate_chat_unique_name_is_busy(Chat entity, int statusCode)
    {
        ChatValidator sut = new();

        Result result = sut.ValidateHasChatUniqueName(entity);

        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetChatWithOwner))]
    public void Validate_chat_to_null_or_deleted_and_owner(Chat entity, Guid ownerId, int statusCode)
    {
        ChatValidator sut = new();

        Result result = sut.ValidateChatAndOwner(entity, ownerId);

        Assert.Equal(statusCode, result.StatusCode);
    }

    public static IEnumerable<object[]> GetChat()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new Chat { IsDeleted = true }, 400 };
        yield return new object[] { new Chat { IsDeleted = false }, 200 };
    }
    public static IEnumerable<object[]> GetChatType()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new ChatType { IsDeleted = true }, 400 };
        yield return new object[] { new ChatType { IsDeleted = false }, 200 };
    }
    public static IEnumerable<object[]> GetChatForUniqueName()
    {
        yield return new object[] { null!, 200 };
        yield return new object[] { new Chat { IsDeleted = true }, 200 };
        yield return new object[] { new Chat { IsDeleted = false }, 400 };
    }
    public static IEnumerable<object[]> GetChatWithOwner()
    {
        yield return new object[] { null!, Guid.Empty, 404 };
        yield return new object[] { new Chat { IsDeleted = true }, Guid.Empty, 400 };
        yield return new object[] { new Chat { IsDeleted = true, OwnerId = Guid.Empty }, Guid.Empty, 400 };
        yield return new object[] { new Chat { IsDeleted = false, OwnerId = Guid.CreateVersion7() }, Guid.Empty, 403 };
        yield return new object[] { new Chat { IsDeleted = false, OwnerId = Guid.Empty }, Guid.Empty, 200 };
    }
}
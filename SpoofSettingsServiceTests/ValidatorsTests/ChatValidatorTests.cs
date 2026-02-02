using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.ServiceRealizations.Validators;

namespace SpoofSettingsServiceTests.ValidatorsTests;

public class ChatValidatorTests 
{
    [Theory]
    [MemberData(nameof(GetChat), DisableDiscoveryEnumeration = true)]
    public void Validate_chat_to_null_or_deleted(Chat entity, int statusCode)
    {
        ChatValidator sut = new();

        Result result = sut.IsAvailable(entity);

        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetChatType), DisableDiscoveryEnumeration = true)]
    public void Validate_chatType_to_null_or_deleted(ChatType entity, int statusCode)
    {
        ChatValidator sut = new();

        Result result = sut.ValidateChatType(entity);

        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetChatForUniqueName), DisableDiscoveryEnumeration = true)]
    public void Validate_chat_unique_name_is_busy(Chat entity, int statusCode)
    {
        ChatValidator sut = new();

        Result result = sut.ValidateHasChatUniqueName(entity);

        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetChatWithOwner), DisableDiscoveryEnumeration = true)]
    public void Validate_chat_to_null_or_deleted_and_owner(Chat entity, Guid ownerId, int statusCode)
    {
        ChatValidator sut = new();

        Result result = sut.ValidateChatAndOwner(entity, ownerId);

        Assert.Equal(statusCode, result.StatusCode);
    }

    public static TheoryData<Chat, int> GetChat()
    {
        TheoryData<Chat, int> data = [];
        data.Add(null!, 404);
        data.Add(new Chat { IsDeleted = true }, 400);
        data.Add(new Chat { IsDeleted = false }, 200);
        return data;
    }
    public static TheoryData<ChatType, int> GetChatType()
    {
        TheoryData<ChatType, int> data = [];
        data.Add(null!, 404 );
        data.Add(new() { IsDeleted = true }, 400 );
        data.Add(new() { IsDeleted = false }, 200);
        return data;
    }
    public static TheoryData<Chat, int> GetChatForUniqueName()
    {
        TheoryData<Chat, int> data = [];
        data.Add(null!, 200);
        data.Add(new() { IsDeleted = true }, 200);
        data.Add(new() { IsDeleted = false }, 400);
        return data;
    }
    public static TheoryData<Chat, Guid, int> GetChatWithOwner()
    {
        TheoryData<Chat, Guid, int> data = [];
        data.Add(null!, Guid.Empty, 404);
        data.Add(new() { IsDeleted = true }, Guid.Empty, 400);
        data.Add(new() { IsDeleted = true, OwnerId = Guid.Empty }, Guid.Empty, 400);
        data.Add(new() { IsDeleted = false, OwnerId = Guid.CreateVersion7() }, Guid.Empty, 403);
        data.Add(new() { IsDeleted = false, OwnerId = Guid.Empty }, Guid.Empty, 200);
        return data;
    }
}
using CommonObjects.Results;
using SpoofMessageService.Models;
using SpoofMessageService.ServiceRealizations.Validators;

namespace SpoofMessageServiceTests.ValidatorsTests;

public class MessageValidatorTests
{
    [Theory]

    [MemberData(nameof(GetMessagesWithOwner))]
    public void Validate_trust_edit_message(Message entity, Guid? userId, int statusCode)
    {
        MessageValidator sut = new();

        Result result = sut.IsAvailableAndOwner(entity, userId);

        Assert.Equal(statusCode, result.StatusCode);
    }

    public static IEnumerable<object[]> GetMessagesWithOwner()
    {
        yield return new object[] { null!, null!, 404 };
        yield return new object[] { null!, Guid.Empty, 404 };
        yield return new object[] { new Message { IsDeleted = true, UserId = Guid.NewGuid() }, null!, 400 };
        yield return new object[] { new Message { IsDeleted = true, UserId = Guid.NewGuid() }, Guid.Empty, 400 };
        yield return new object[] { new Message { IsDeleted = true, UserId = Guid.Empty }, null!, 400 };
        yield return new object[] { new Message { IsDeleted = true, UserId = Guid.Empty }, Guid.Empty, 400 };
        yield return new object[] { new Message { IsDeleted = true }, null!, 400 };
        yield return new object[] { new Message { IsDeleted = true }, Guid.Empty, 400 };
        yield return new object[] { new Message { IsDeleted = true }, Guid.NewGuid(), 400 };
        yield return new object[] { new Message { IsDeleted = false, UserId = Guid.NewGuid() }, null!, 403 };
        yield return new object[] { new Message { IsDeleted = false, UserId = Guid.NewGuid() }, Guid.Empty, 403 };
        yield return new object[] { new Message { IsDeleted = false, UserId = Guid.Empty }, null!, 403 };
        yield return new object[] { new Message { IsDeleted = false, UserId = Guid.Empty }, Guid.Empty, 200 };
        yield return new object[] { new Message { IsDeleted = false }, null!, 403 };
        yield return new object[] { new Message { IsDeleted = false }, Guid.NewGuid(), 403 };
    }
}

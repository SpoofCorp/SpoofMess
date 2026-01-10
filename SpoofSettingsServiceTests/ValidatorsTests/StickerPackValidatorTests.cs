using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.ServiceRealizations.Validators;

namespace SpoofSettingsServiceTests.ValidatorsTests;

public class StickerPackValidatorTests
{
    [Theory]
    [MemberData(nameof(GetStickerPacks))]
    public void Validate_stickerpacks_to_null_or_deleted(StickerPack entity, int statusCode)
    {
        StickerPackValidator sut = new();

        Result result = sut.IsAvailable(entity);

        Assert.Equal(statusCode, result.StatusCode);
    }

    [Theory]
    [MemberData(nameof(GetStickerPacksWithOwner))]
    public void Validate_stickerpacks_to_null_or_deleted_and_check_author(StickerPack entity, Guid authorId, int statusCode)
    {
        StickerPackValidator sut = new();

        Result result = sut.IsOwner(entity, authorId);

        Assert.Equal(statusCode, result.StatusCode);
    }

    public static IEnumerable<object[]> GetStickerPacks()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new StickerPack { IsDeleted = true }, 400 };
        yield return new object[] { new StickerPack { IsDeleted = false }, 200 };
    }
    public static IEnumerable<object[]> GetStickerPacksWithOwner()
    {
        yield return new object[] { null!, Guid.Empty, 404 };
        yield return new object[] { new StickerPack { IsDeleted = true, AuthorId = Guid.Empty }, Guid.Empty, 400 };
        yield return new object[] { new StickerPack { IsDeleted = true, AuthorId = Guid.CreateVersion7() }, Guid.Empty, 400 };
        yield return new object[] { new StickerPack { IsDeleted = false, AuthorId = Guid.CreateVersion7() }, Guid.Empty, 403 };
        yield return new object[] { new StickerPack { IsDeleted = false, AuthorId = Guid.Empty }, Guid.Empty, 200 };
    }
}

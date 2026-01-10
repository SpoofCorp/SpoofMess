using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.ServiceRealizations.Validators;

namespace SpoofSettingsServiceTests.ValidatorsTests;

public class StickerValidatorTests
{
    [Theory]
    [MemberData(nameof(GetStickers))]
    public void Validate_sticker_to_null_or_deleted_and_null_file(Sticker entity, int statusCode)
    {
        StickerValidator sut = new();

        Result result = sut.IsAvailable(entity);

        Assert.Equal(statusCode, result.StatusCode);
    }

    public static IEnumerable<object[]> GetStickers()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new Sticker { IsDeleted = true, FileId = null }, 400 };
        yield return new object[] { new Sticker { IsDeleted = true, FileId = Guid.Empty }, 400 };
        yield return new object[] { new Sticker { IsDeleted = false, FileId = null }, 400 };
        yield return new object[] { new Sticker { IsDeleted = false, FileId = Guid.Empty }, 200 };
    }
}

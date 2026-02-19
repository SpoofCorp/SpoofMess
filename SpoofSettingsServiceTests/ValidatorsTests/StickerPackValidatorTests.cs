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

    public static TheoryData<StickerPack, int> GetStickerPacks()
    {
        TheoryData<StickerPack, int> data = [];
        data.Add(null!, 404);
        data.Add(new StickerPack { IsDeleted = true }, 400);
        data.Add(new StickerPack { IsDeleted = false }, 200);
        return data;
    }
    public static TheoryData<StickerPack, Guid, int> GetStickerPacksWithOwner()
    {
        TheoryData<StickerPack, Guid, int> data = [];
        data.Add(null!, Guid.Empty, 404);
        data.Add(new StickerPack { IsDeleted = true, AuthorId = Guid.Empty }, Guid.Empty, 400);
        data.Add(new StickerPack { IsDeleted = true, AuthorId = Guid.CreateVersion7() }, Guid.Empty, 400);
        data.Add(new StickerPack { IsDeleted = false, AuthorId = Guid.CreateVersion7() }, Guid.Empty, 403);
        data.Add(new StickerPack { IsDeleted = false, AuthorId = Guid.Empty }, Guid.Empty, 200);
        return data;
    }
}

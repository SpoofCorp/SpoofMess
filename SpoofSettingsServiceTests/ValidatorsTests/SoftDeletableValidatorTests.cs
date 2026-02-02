using CommonObjects.Results;
using DataSaveHelpers.EntityTypes;
using DataSaveHelpers.ServiceRealizations;

namespace SpoofSettingsServiceTests.ValidatorsTests;

public class SoftDeletableValidatorTests
{
    [Theory]
    [MemberData(nameof(GetCollections))]
    public void Validate_collections_to_null_or_empty(List<SoftDeletable> entities, int statusCode)
    {
        SoftDeletableValidator<SoftDeletable> sut = new();

        Result result = sut.IsAvailableCollection(entities);

        Assert.Equal(statusCode, result.StatusCode);
    }
    [Theory]

    [MemberData(nameof(GetSoftDeletableEntities))]
    public void Validate_trust_session_infos(SoftDeletable entity, int statusCode)
    {
        SoftDeletableValidator<SoftDeletable> sut = new();

        Result result = sut.IsAvailable(entity);

        Assert.Equal(statusCode, result.StatusCode);
    }

    public static IEnumerable<object[]> GetSoftDeletableEntities()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new SoftDeletable { IsDeleted = true }, 400 };
        yield return new object[] { new SoftDeletable { IsDeleted = false }, 200 };
    }
    public static IEnumerable<object[]> GetCollections()
    {
        yield return new object[] { null!, 404 };
        yield return new object[] { new List<SoftDeletable>(), 400 };
        yield return new object[] { new List<SoftDeletable>() { new() }, 200 };
        yield return new object[] { new List<SoftDeletable>() { new(), new(), new(), new() }, 200 };
    }

    public class SoftDeletable : ISoftDeletable
    {
        public bool IsDeleted { get; set; }
    }
}

using CommonObjects.Results;
using DataSaveHelpers.EntityTypes;
using DataSaveHelpers.ServiceRealizations;

namespace SpoofMessageServiceTests.ValidatorsTests;

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

    public static TheoryData<SoftDeletable, int> GetSoftDeletableEntities()
    {
        TheoryData<SoftDeletable, int> data = [];
        data.Add(null!, 404);
        data.Add(new SoftDeletable { IsDeleted = true }, 400);
        data.Add(new SoftDeletable { IsDeleted = false }, 200);
        return data;
    }
    public static TheoryData<List<SoftDeletable>, int> GetCollections()
    {
        TheoryData<List<SoftDeletable>, int> data = [];
        data.Add(null!, 404);
        data.Add([], 400);
        data.Add([new()], 200);
        data.Add([new(), new(), new(), new()], 200);
        return data;
    }

    public class SoftDeletable : ISoftDeletable
    {
        public bool IsDeleted { get; set; }
    }
}

namespace DataSaveHelpers.Services;

public interface IMemoryCacheService : ICacheService
{
    public Task Clear();
}

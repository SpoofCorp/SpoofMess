namespace DataHelpers.Services;

public interface IRedisService
{
    public Task Save<T>(string key, T value);

    public Task SaveRange<T>(Func<T, string> getKey, List<T> values);

    public Task Delete(string key);

    public Task<T?> Get<T>(string key);
}
using AdditionalHelpers.Services;
using DataSaveHelpers.Services;
using Microsoft.Extensions.Caching.Memory;

namespace DataSaveHelpers.ServiceRealizations.Cache.Memory;

public class LocalCacheService(IMemoryCache cache, ILoggerService loggerService) : IMemoryCacheService, IDisposable
{
    private readonly IMemoryCache _cache = cache;
    private readonly ILoggerService _loggerService = loggerService;

    public Task Clear()
    {
        _cache.Dispose();
        return Task.CompletedTask;
    }

    public Task Delete(string key)
    {
        _cache.Remove(key);
        _loggerService.Debug($"Delete by {key} in cache");
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _cache.Dispose();
        GC.SuppressFinalize(this);
    }

    public Task<T?> Get<T>(string key)
    {
        _cache.TryGetValue(key, out T? value);
        _loggerService.Debug($"Get by {key} from cache");
        return Task.FromResult(value);
    }

    public Task Save<T>(string key, T value)
    {
        _cache.Set(key, value);
        _loggerService.Debug($"Save by {key} to cache");
        return Task.CompletedTask;
    }

    public async Task SaveRange<T>(Func<T, string> getKey, List<T> values)
    {
        T value;
        for(int i = 0; i < values.Count; i++)
        {
            value = values[i];
            await Save(getKey(value), value);
        }
    }
}

using AdditionalHelpers.Services;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataSaveHelpers.ServiceRealizations.Cache;

public class MultiCache(IMemoryCacheService localCache, IRedisService cache, ILoggerService loggerService) : ICacheService
{
    protected readonly IRedisService _cache = cache;
    protected readonly IMemoryCacheService _localCache = localCache;
    private readonly ILoggerService _loggerService = loggerService;

    public async Task Save<T>(string key, T value)
    {
        await _localCache.Save(key, value);
        _loggerService.Debug($"Save by {key} to in-memory cache");
        await _cache.Save(key, value);
        _loggerService.Debug($"Save by {key} to redis");
    }

    public async Task Delete(string key)
    {
        await _localCache.Delete(key);
        _loggerService.Debug($"Delete by {key} to in-memory cache");
        await _cache.Delete(key);
        _loggerService.Debug($"Delete by {key} to redis");
    }

    public async Task<T?> Get<T>(string key)
    {
        T? entity = await _localCache.Get<T?>(key);
        if (entity is not null)
        {
            _loggerService.Debug($"Received by {key} from in-memory cache");
            return entity;
        }

        entity = await _cache.Get<T?>(key);
        if (entity is not null)
        {
            _loggerService.Debug($"Received by {key} from redis");
            await _localCache.Save(key, entity);
        }

        return entity;
    }

    public async Task SaveRange<T>(Func<T, string> getKey, List<T> values)
    {
        await _localCache.SaveRange(getKey, values);
        _loggerService.Debug($"Save collection {values.GetType().Name} to in-memory cache");
        await _cache.SaveRange(getKey, values);
        _loggerService.Debug($"Save by {values.GetType().Name} to redis");
    }
}

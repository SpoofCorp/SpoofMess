namespace DataHelpers.Services.Repositories;

public abstract class CachedRepository<TEntity>
{
    protected abstract ValueTask<TEntity?> GetAsync(string key, Func<Task<TEntity?>> function, TimeSpan? expiration = null);

    protected abstract void SaveToCache(string key, TEntity entity);

    protected abstract void SaveRangeToCache(List<TEntity> entities);
}
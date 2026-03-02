using DataSaveHelpers.Services;
using DataSaveHelpers.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;

public abstract class CachedFactoryBaseRepository<T, TDbContext>(
    ICacheService cache,
    IDbContextFactory<TDbContext> factory,
    IProcessQueueTasksService processQueueTasks
    ) : CachedRepository<T> where T : class
    where TDbContext : DbContext
{
    protected readonly IProcessQueueTasksService _processQueueTasks = processQueueTasks;
    protected readonly ICacheService _cache = cache;
    protected readonly IDbContextFactory<TDbContext> _factory = factory;

    public async Task AddAsync(T entity)
    {
        try
        {
            await using DbContext context = await _factory.CreateDbContextAsync();
            DbSet<T> set = context.Set<T>();
            await set.AddAsync(entity);
            await context.SaveChangesAsync();
            await _cache.Save(GetKey(entity), entity);
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }

    public async Task DeleteAsync(T entity)
    {
        try
        {
            await using DbContext context = await _factory.CreateDbContextAsync();
            DbSet<T> set = context.Set<T>();
            await _cache.Delete(GetKey(entity));
            set.Remove(entity);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }

    public async Task UpdateAsync(T entity)
    {
        try
        {
            await using DbContext context = await _factory.CreateDbContextAsync();
            DbSet<T> set = context.Set<T>();
            await _cache.Save(GetKey(entity), entity);
            set.Update(entity);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }

    public async Task UpdateRangeAsync(List<T> entities)
    {
        try
        {
            await using DbContext context = await _factory.CreateDbContextAsync();
            DbSet<T> set = context.Set<T>();
            await _cache.SaveRange(GetKey, entities);
            set.UpdateRange(entities);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }

    protected async ValueTask<T?> GetFromDb(string key, Func<Task<T?>> function)
    {
        T? entity = await function();
        if (entity is not null)
            _processQueueTasks.AddTask(async () => await _cache.Save(key, entity));

        return entity;
    }
    protected override async ValueTask<T?> GetAsync(string key, Func<Task<T?>> function, TimeSpan? expiration = null)
    {
        try
        {
            T? entity = await _cache.Get<T>(key);

            entity ??= await GetFromDb(key, function);

            return entity;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("DataBase error", ex);
        }
    }

    protected abstract string GetKey(T entity);

    protected override void SaveToCache(string key, T entity) =>
        _processQueueTasks.AddTask(async () => await _cache.Save(key, entity));

    protected void SaveEntityToCache<TEntity>(string key, TEntity entity) =>
        _processQueueTasks.AddTask(async () => await _cache.Save(key, entity));

    protected override void SaveRangeToCache(List<T> entities) =>
        _processQueueTasks.AddTask(async () => await _cache.SaveRange(GetKey, entities));

}

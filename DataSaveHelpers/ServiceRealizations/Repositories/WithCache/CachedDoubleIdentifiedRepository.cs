using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;
using DataSaveHelpers.Services;
using DataSaveHelpers.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataSaveHelpers.ServiceRealizations.Repositories.WithCache;

public class CachedDoubleIdentifiedRepository<T, TKey1, TKey2>(ICacheService cache, DbContext context, IProcessQueueTasksService processQueueTasks) : CachedBaseRepository<T>(cache, context, processQueueTasks), IDoubleIdentifiedRepository<T, TKey1, TKey2> where T : DoubleIdentifiedEntity<TKey1, TKey2>
{
    public async Task<T?> GetByIdAsync(TKey1 key1, TKey2 key2)
    {
        try
        {
            T? entity = await _cache.Get<T>(GetKey(key1, key2));
            entity ??= await _set.FirstOrDefaultAsync(x => x.Key1!.Equals(key1) && x.Key2!.Equals(key2));

            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }
    protected override string GetKey(T entity) =>
        $"{entity.GetType().Name.ToLower()}:{entity.Key1}:{entity.Key2}";
    protected string GetKey(TKey1 key1, TKey2 key2) =>
        $"{typeof(T).Name.ToLower()}:{key1}:{key2}";
}

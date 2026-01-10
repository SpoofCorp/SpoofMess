using DataHelpers.Services;
using DataHelpers.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataHelpers.ServiceRealizations.Repositories.WithCache;

public class CachedIdentifiedRepository<T, TKey>(ICacheService cache, DbContext context, IProcessQueueTasksService processQueueTasks) : CachedBaseRepository<T>(cache, context, processQueueTasks), IIdentifiedRepository<T, TKey> where T : IdentifiedEntity<TKey>
{
    public async ValueTask<T?> GetByIdAsync(TKey id)
    {
        try
        {
            T? entity = await _cache.Get<T>(GetKey(id));
            entity ??= await _set.FirstOrDefaultAsync(x => x.Id!.Equals(id));

            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }
    protected override string GetKey(T entity) =>
        $"{entity.GetType().Name.ToLower()}:{entity.Id}";

    protected virtual string GetKey(TKey id) =>
        $"{typeof(T).Name.ToLower()}:{id}";
}

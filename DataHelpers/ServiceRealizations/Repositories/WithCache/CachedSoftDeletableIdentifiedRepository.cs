using DataHelpers.ServiceRealizations.Repositories.OnlyDb;
using DataHelpers.Services;
using DataHelpers.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataHelpers.ServiceRealizations.Repositories.WithCache;

public class CachedSoftDeletableIdentifiedRepository<T, TKey>(ICacheService cache, DbContext context, IProcessQueueTasksService processQueueTasks) : CachedIdentifiedRepository<T, TKey>(cache, context, processQueueTasks), ISoftDeletableIdentifiedRepository<T, TKey> where T : IdentifiedSoftDeletableEntity<TKey>
{
    public async Task SoftDeleteAsync(T entity)
    {
        try
        {
            entity.IsDeleted = true;
            await _cache.Save(GetKey(entity), entity);
            _set.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }

    public async Task<bool> SoftDeleteAsync(TKey id)
    {
        try
        {
            T? entity = await GetByIdAsync(id);
            if (entity is null) return false;

            await SoftDeleteAsync(entity);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }
}
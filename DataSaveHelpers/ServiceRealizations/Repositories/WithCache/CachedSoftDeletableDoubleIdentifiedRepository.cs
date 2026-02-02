using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;
using DataSaveHelpers.Services;
using DataSaveHelpers.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataSaveHelpers.ServiceRealizations.Repositories.WithCache;

public class CachedSoftDeletableDoubleIdentifiedRepository<T, TKey1, TKey2>(ICacheService cache, DbContext context, IProcessQueueTasksService processQueueTasks) : CachedDoubleIdentifiedRepository<T, TKey1, TKey2>(cache, context, processQueueTasks), ISoftDeletableDoubleIdentifiedRepository<T, TKey1, TKey2> where T : DoubleIdentifiedSoftDeletable<TKey1, TKey2>
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

    public async Task<bool> SoftDeleteAsync(TKey1 key1, TKey2 key2)
    {
        try
        {
            T? entity = await GetByIdAsync(key1, key2);
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
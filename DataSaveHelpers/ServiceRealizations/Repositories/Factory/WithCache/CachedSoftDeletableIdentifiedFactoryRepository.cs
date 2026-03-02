using DataSaveHelpers.EntityTypesRealizations.Identified;
using DataSaveHelpers.Services;
using DataSaveHelpers.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;

public class CachedSoftDeletableIdentifiedFactoryRepository<T, TKey, TDbContext>(
        ICacheService cache,
        IDbContextFactory<TDbContext> factory,
        IProcessQueueTasksService processQueueTasks
    ) : CachedIdentifiedFactoryRepository<T, TKey, TDbContext>(
            cache,
            factory,
            processQueueTasks
        ), ISoftDeletableIdentifiedRepository<T, TKey> where T : IdentifiedSoftDeletableEntity<TKey> 
    where TDbContext : DbContext
{
    public async Task SoftDeleteAsync(T entity)
    {
        try
        {
            entity.IsDeleted = true;
            await using DbContext context = await _factory.CreateDbContextAsync();
            context.Entry(entity).State = EntityState.Modified;
            await _cache.Save(GetKey(entity), entity);
            await context.SaveChangesAsync();
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
using DataSaveHelpers.EntityTypesRealizations.SoftDeletable;
using DataSaveHelpers.Services;
using DataSaveHelpers.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;

public abstract class CachedSoftDeletableFactoryRepository<T, TDbContext>(
    ICacheService cache,
    IDbContextFactory<TDbContext> factory,
    IProcessQueueTasksService processQueueTasks
    ) : CachedFactoryBaseRepository<T, TDbContext>(
        cache,
        factory,
        processQueueTasks
        ), ISoftDeletableRepository<T> 
    where T : SoftDeletableEntity
    where TDbContext : DbContext
{
    public async Task SoftDeleteAsync(T entity)
    {
        try
        {
            entity.IsDeleted = true;
            await using DbContext context = await _factory.CreateDbContextAsync();
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            await _cache.Save(GetKey(entity), entity);
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }
}
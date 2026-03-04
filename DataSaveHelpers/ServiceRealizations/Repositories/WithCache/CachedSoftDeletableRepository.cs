using DataSaveHelpers.EntityTypesRealizations.SoftDeletable;
using DataSaveHelpers.Services;
using DataSaveHelpers.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataSaveHelpers.ServiceRealizations.Repositories.WithCache;

public abstract class CachedSoftDeletableRepository<T>(
    ICacheService cache,
    DbContext context,
    IProcessQueueTasksService processQueueTasks
    ) : CachedBaseRepository<T>(
        cache,
        context, 
        processQueueTasks
        ), ISoftDeletableRepository<T> where T : SoftDeletableEntity
{
    public async Task SoftDeleteAsync(T entity)
    {
        try
        {
            entity.IsDeleted = true;
            _set.Update(entity);
            await _context.SaveChangesAsync();
            await _cache.Save(GetKey(entity), entity);
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }
}
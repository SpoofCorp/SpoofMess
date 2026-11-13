using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class StickerRepository(
    ICacheService cache,
    IDbContextFactory<SpoofSettingsServiceContext> factory,
    IProcessQueueTasksService tasksService
    ) : CachedSoftDeletableIdentifiedFactoryRepository<Sticker, Guid, SpoofSettingsServiceContext>(
        cache, 
        factory, 
        tasksService
    ), IStickerRepository
{

    public async Task<bool> SoftDeleteAsync(string key)
    {
        try
        {
            Sticker? entity = await _cache.Get<Sticker>(GetKey(key));
            await using DbContext context = await _factory.CreateDbContextAsync();
            entity ??= await context.Set<Sticker>().FirstOrDefaultAsync(x => x.Key == key);
            if (entity is null)
                return false;

            entity.IsDeleted = true;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            await _cache.Save(GetKey(entity), entity);

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }

    protected string GetKey(string key) =>
        $"{typeof(Sticker).Name.ToLower()}:{key}";

    protected override string GetKey(Sticker entity) =>
        GetKey(entity.Key);
}
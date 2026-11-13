using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class StickerPackRepository(
        ICacheService cache,
        IDbContextFactory<SpoofSettingsServiceContext> factory,
        IProcessQueueTasksService tasksService
    ) : CachedSoftDeletableIdentifiedFactoryRepository<StickerPack, long, SpoofSettingsServiceContext>(
        cache, 
        factory, 
        tasksService
    ), IStickerPackRepository
{
    public async Task<StickerPack?> GetByKeyAsync(string key)
    {
        await using SpoofSettingsServiceContext context = await _factory.CreateDbContextAsync();
        return await context.StickerPacks.FirstOrDefaultAsync(x => x.Key == key);
    }

    public async Task<StickerPack?> GetWithStickers(string key)
    {
        await using SpoofSettingsServiceContext context = await _factory.CreateDbContextAsync();
        return await context.StickerPacks.Include(x => x.Stickers).FirstOrDefaultAsync(x => x.Key == key);
    }
}

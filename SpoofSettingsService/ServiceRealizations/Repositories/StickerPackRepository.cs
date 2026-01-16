using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class StickerPackRepository(ICacheService cache, SpoofSettingsServiceContext context, IProcessQueueTasksService tasksService) : CachedSoftDeletableIdentifiedRepository<StickerPack, long>(cache, context, tasksService), IStickerPackRepository
{
    public async Task<StickerPack?> GetWithStickers(long id) =>
        await _set.Include(x => x.Stickers).FirstOrDefaultAsync(x => x.Id == id);
}

using DataHelpers.ServiceRealizations;
using DataHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class StickerPackRepository(ICacheService cache, SpoofSettingsServiceContext context, ProcessQueueTasksService tasksService) : Repository<StickerPack, Guid>(cache, context, tasksService), IStickerPackRepository
{
    public async Task<StickerPack?> GetWithStickers(Guid id) =>
        await _set.Include(x => x.Stickers).FirstOrDefaultAsync(x => x.Id == id);
}

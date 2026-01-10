using DataHelpers.ServiceRealizations.Repositories.WithCache;
using DataHelpers.Services;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class StickerRepository(ICacheService cache, SpoofSettingsServiceContext context, IProcessQueueTasksService tasksService) : CachedSoftDeletableIdentifiedRepository<Sticker, Guid>(cache, context, tasksService), IStickerRepository
{
}
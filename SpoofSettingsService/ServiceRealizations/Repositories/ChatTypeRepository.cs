using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class ChatTypeRepository(
        ICacheService cacheService,
        IDbContextFactory<SpoofSettingsServiceContext> factory,
        IProcessQueueTasksService queueTasksService
    ) : CachedSoftDeletableIdentifiedFactoryRepository<ChatType, int, SpoofSettingsServiceContext>(
        cacheService,
        factory,
        queueTasksService
    ), IChatTypeRepository
{
}

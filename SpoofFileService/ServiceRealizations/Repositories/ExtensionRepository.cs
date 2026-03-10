using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofFileService.Models;
using SpoofFileService.Services.Repositories;

namespace SpoofFileService.ServiceRealizations.Repositories;

public class ExtensionRepository(
    ICacheService cache,
    IDbContextFactory<SpoofFileServiceContext> factory,
    IProcessQueueTasksService processQueueTasks) : CachedSoftDeletableIdentifiedFactoryRepository<Extension, short, SpoofFileServiceContext>(
        cache,
        factory,
        processQueueTasks), IExtensionRepository
{
    public async Task<Extension?> GetByName(string name)
    {
        await using SpoofFileServiceContext context = await _factory.CreateDbContextAsync();
        return await context.Extensions.FirstOrDefaultAsync(x => x.Name == name);
    }
}

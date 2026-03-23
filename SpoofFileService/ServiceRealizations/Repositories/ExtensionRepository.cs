using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofFileParser;
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
    public async Task<ExtensionDto?> GetByName(FileExtension fileExtension)
    {
        await using SpoofFileServiceContext context = await _factory.CreateDbContextAsync();
        return await context.Set<ExtensionDto>()
            .FromSqlInterpolated($@"SELECT * FROM ""FindOrCreateExtension""({fileExtension.Id}, {fileExtension.Name}, {fileExtension.Type.ToString()})")
            .SingleAsync();
    }
}

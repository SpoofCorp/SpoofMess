using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofFileInfo;
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
    public async Task<Extension?> GetByName(FileExtension2 fileExtension2)
    {
        await using SpoofFileServiceContext context = await _factory.CreateDbContextAsync();
        return await context.Extensions.FromSql(@$"SELECT * FROM ""FindOrCreateExtension""({fileExtension2.Id}, {fileExtension2.Name}, {fileExtension2.Type.ToString()})").SingleAsync();
    }
}

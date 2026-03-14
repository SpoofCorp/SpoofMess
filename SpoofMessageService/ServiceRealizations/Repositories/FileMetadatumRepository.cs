using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofMessageService.Models;
using SpoofMessageService.Services.Repositories;

namespace SpoofMessageService.ServiceRealizations.Repositories;

public class FileMetadatumRepository(
    ICacheService cache,
    IDbContextFactory<SpoofMessageServiceContext> factory,
    IProcessQueueTasksService processQueueTasks) : CachedSoftDeletableIdentifiedFactoryRepository<FileMetadatum, Guid, SpoofMessageServiceContext>(
        cache, 
        factory, 
        processQueueTasks), IFileMetadatumRepository
{
    public new async Task<FileMetadatum?> GetByIdAsync(Guid id)
    {
        await using SpoofMessageServiceContext context = await _factory.CreateDbContextAsync();
        return await context.FileMetadata
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}

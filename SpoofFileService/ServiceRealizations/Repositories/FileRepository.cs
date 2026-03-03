using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofFileService.Models;
using SpoofFileService.Services.Repositories;

namespace SpoofFileService.ServiceRealizations.Repositories;

public class FileRepository(
    ICacheService cache,
    IDbContextFactory<SpoofFileServiceContext> factory,
    IProcessQueueTasksService processQueueTasks
    ) : CachedSoftDeletableIdentifiedFactoryRepository<FileObject, byte[], SpoofFileServiceContext>(
        cache,
        factory, 
        processQueueTasks
    ), IFileRepository
{
    public async Task<bool> Save(FileObject fileObject)
    {
        await using SpoofFileServiceContext context = await _factory.CreateDbContextAsync();
        return await context.Database.SqlQuery<bool>($@"SELECT ""FindOrCreateFile""({fileObject.Id},{fileObject.L1},{fileObject.L2},{fileObject.CategoryId},{fileObject.ExtensionId}, {fileObject.Path}, {fileObject.Size}) AS ""Value""").SingleAsync();
    }
}

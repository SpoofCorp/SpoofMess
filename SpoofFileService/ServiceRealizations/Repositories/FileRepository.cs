using CommonObjects.Requests.Files;
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
    public async Task<bool> ExistByL3(FingerprintExistL3 l3)
    {
        await using SpoofFileServiceContext context = await _factory.CreateDbContextAsync();
        return await context.FileObjects.AnyAsync(x => x.Id == l3.Fingerprint && x.Size == l3.FileSize);
    }

    public async Task<bool> PreExistByL1AndL2(FingerprintExistL1L2 fingerprint)
    {
        await using SpoofFileServiceContext context = await _factory.CreateDbContextAsync();
        return await context.FileObjects.AnyAsync(x => x.L1 == fingerprint.L1 && x.L2 == fingerprint.L2 && x.Size == fingerprint.FileSize);
    }
}

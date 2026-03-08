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
    ) : CachedSoftDeletableIdentifiedFactoryRepository<FileObject, Guid, SpoofFileServiceContext>(
        cache,
        factory, 
        processQueueTasks
    ), IFileRepository
{
    public async Task<bool> Save(FileObject fileObject)
    {
        await using SpoofFileServiceContext context = await _factory.CreateDbContextAsync();
        return await context.Database.SqlQuery<bool>($@"SELECT ""FindOrCreateFile""({fileObject.Id},{fileObject.L1},{fileObject.L2},{fileObject.ExtensionId}, {fileObject.Path}, {fileObject.Size}) AS ""Value""").SingleAsync();
    }
    public async Task<bool> ExistByL3(FingerprintExistL3 l3)
    {
        await using SpoofFileServiceContext context = await _factory.CreateDbContextAsync();
        return await context.FileObjects
            .Include(x => x.Extension)
            .AnyAsync(x => 
                x.L3 == l3.Fingerprint 
                && x.Size == l3.Metadata.Size
                && x.ExtensionId == l3.Metadata.ExtentisionId
            );
    }

    public async Task<bool> PreExistByL1AndL2(FingerprintExistL1L2 fingerprint)
    {
        await using SpoofFileServiceContext context = await _factory.CreateDbContextAsync();
        return await context.FileObjects
            .Include(x => x.Extension)
            .AnyAsync(x => 
                x.L1 == fingerprint.L1 
                && x.L2 == fingerprint.L2 
                && x.Size == fingerprint.Metadata.Size
                && x.ExtensionId == fingerprint.Metadata.ExtentisionId
            );
    }

    public async Task<FileObject?> GetByL3(byte[] l3, FileMetadata metadata)
    {
        await using SpoofFileServiceContext context = await _factory.CreateDbContextAsync();
        return await context.FileObjects
            .Include(x => x.Extension)
            .FirstOrDefaultAsync(x => 
                x.L3 == l3 
                && x.Size == metadata.Size
                && x.ExtensionId == metadata.ExtentisionId
            );
    }
}

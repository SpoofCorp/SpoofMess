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
    public async Task<Guid?> Save(FileObject fileObject)
    {
        await using SpoofFileServiceContext context = await _factory.CreateDbContextAsync();
        return await context.Database.SqlQuery<Guid?>(@$"SELECT ""FindOrCreateFile""({fileObject.Id},{fileObject.L1},{fileObject.L2},{fileObject.L3},{fileObject.ExtensionId}, {fileObject.Path}, {fileObject.Size}, {fileObject.Metadata}) AS ""Value""").SingleAsync();
    }
    public async Task<bool> ExistByL3(FingerprintExistL3 l3)
    {
        await using SpoofFileServiceContext context = await _factory.CreateDbContextAsync();
        return await context.FileObjects
            .Include(x => x.Extension)
            .AnyAsync(x => 
                x.L3 == l3.Fingerprint 
                && x.Size == l3.Metadata.Size
                && x.ExtensionId == l3.Metadata.ExtensionId
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
                && x.ExtensionId == fingerprint.Metadata.ExtensionId
            );
    }

    public async Task<FileObject?> GetByL3(byte[] l3, FileMetadata metadata)
    {
        await using SpoofFileServiceContext context = await _factory.CreateDbContextAsync();
        return await context.FileObjects
            .Include(x => x.Extension)
            .FirstOrDefaultAsync(x => 
                x.L3 == l3 
            );
    }
}

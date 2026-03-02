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
}

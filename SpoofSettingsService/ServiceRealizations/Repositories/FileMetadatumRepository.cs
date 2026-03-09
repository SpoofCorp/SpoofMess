using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class FileMetadatumRepository(
    ICacheService cache,
    IDbContextFactory<SpoofSettingsServiceContext> factory, 
    IProcessQueueTasksService tasksService
    ) : CachedSoftDeletableIdentifiedFactoryRepository<FileMetadatum, Guid, SpoofSettingsServiceContext>(
        cache, 
        factory,
        tasksService
        ), IFileMetadatumRepository
{
}

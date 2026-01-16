using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofFileService.Models;
using SpoofFileService.ServiceRealizatoionss.Repositories;

namespace SpoofFileService.ServiceRealizations.Validators;

public class FileRepository(ICacheService cache, DbContext context, IProcessQueueTasksService processQueueTasks) : CachedSoftDeletableIdentifiedRepository<FileObject, Guid>(cache, context, processQueueTasks), IFileRepository
{
}

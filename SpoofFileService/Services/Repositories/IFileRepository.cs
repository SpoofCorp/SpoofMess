using DataSaveHelpers.Services.Repositories;
using SpoofFileService.Models;

namespace SpoofFileService.ServiceRealizatoionss.Repositories;

public interface IFileRepository : ISoftDeletableIdentifiedRepository<FileObject, Guid>
{
}

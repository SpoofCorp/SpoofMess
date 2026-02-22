using DataSaveHelpers.Services.Repositories;
using SpoofFileService.Models;

namespace SpoofFileService.Services.Repositories;

public interface IFileRepository : ISoftDeletableIdentifiedRepository<FileObject, byte[]>
{
}

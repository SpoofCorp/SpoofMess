using CommonObjects.Requests.Files;
using DataSaveHelpers.Services.Repositories;
using SpoofFileService.Models;

namespace SpoofFileService.Services.Repositories;

public interface IFileRepository : ISoftDeletableIdentifiedRepository<FileObject, Guid>
{
    public Task<Guid?> Save(FileObject fileObject);
    public Task<bool> PreExistByL1AndL2(FingerprintExistL1L2 fingerprint);
    public Task<bool> ExistByL3(FingerprintExistL3 l3);
    public Task<FileObject?> GetByL3(byte[] l3, FileMetadata metadata);
}

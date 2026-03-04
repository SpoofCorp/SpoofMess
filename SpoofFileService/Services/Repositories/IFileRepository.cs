using CommonObjects.Requests.Files;
using DataSaveHelpers.Services.Repositories;
using SpoofFileService.Models;

namespace SpoofFileService.Services.Repositories;

public interface IFileRepository : ISoftDeletableIdentifiedRepository<FileObject, byte[]>
{
    public Task<bool> Save(FileObject fileObject);
    public Task<bool> PreExistByL1AndL2(FingerprintExistL1L2 fingerprint);
    public Task<bool> ExistByL3(FingerprintExistL3 l3);
}

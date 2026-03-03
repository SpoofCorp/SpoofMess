using CommonObjects.Results;
using SecurityLibrary;

namespace SpoofFileService.Services;

public interface IFingerprintService
{
    public Task<Result<byte[]>> ExistL1(string filePath);

    public Task<Result<byte[]>> ExistL2(string filePath);

    public Task<Result<FileResult>> ExistFull(IFormFile file);
    public Task<Result<FingerprintFull>> GetFull(IFormFile file);
}

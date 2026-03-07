using CommonObjects.Requests.Files;
using CommonObjects.Results;

namespace SpoofFileService.Services;

public interface IFileService
{
    public ValueTask<Result<FileStream>> GetFile(byte[] token, Guid userId);

    public Task<Result<byte[]>> SaveFile(IFormFile file, Guid userId);

    public Task<Result> DeleteFile(byte[] token);
    public Task<Result> ExistL1(FingerprintExistL1L2 request);
    public Task<Result<byte[]>> ExistL3(FingerprintExistL3 request, Guid userId);
}

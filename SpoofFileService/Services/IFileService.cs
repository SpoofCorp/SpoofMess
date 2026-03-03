using CommonObjects.Results;

namespace SpoofFileService.Services;

public interface IFileService
{
    public ValueTask<Result<FileStream>> GetFile(byte[] fileId, Guid userId);

    public Task<Result<byte[]>> SaveFile(IFormFile formFile);

    public Task<Result> DeleteFile(byte[] fileId);
}

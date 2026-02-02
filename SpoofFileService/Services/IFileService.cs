using CommonObjects.Results;

namespace SpoofFileService.Services;

public interface IFileService
{
    public ValueTask<Result<FileStream>> GetFile(Guid fileId, Guid userId);

    public Task<Result> SaveFile(IFormFile formFile, Guid? fileId);

    public Task<Result> DeleteFile(Guid fileId);
}

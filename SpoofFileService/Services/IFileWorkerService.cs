namespace SpoofFileService.Services;

public interface IFileWorkerService
{
    public Task<string> Save(IFormFile file);

    public Task<bool> Delete(string filePath);

    public Task<FileStream?> Get(string filePath);
}

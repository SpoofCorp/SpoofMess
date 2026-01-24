using SpoofFileService.Services;

namespace SpoofFileService.ServiceRealizations;

public class LocalFileWorkerService : IFileWorkerService
{
    public Task<bool> Delete(string filePath)
    {
        if (!File.Exists(filePath))
            return Task.FromResult(false);

        File.Delete(filePath);
        return Task.FromResult(true);
    }

    public Task<FileStream?> Get(string filePath)
    {
        if(File.Exists(filePath))
            return Task.FromResult<FileStream?>(File.OpenRead(filePath));

        return Task.FromResult<FileStream?>(null);
    }

    public async Task<string> Save(IFormFile file)
    {
        string filePath = GetFilePath(file.FileName, $@"{Directory.GetCurrentDirectory()}\Storage\");
        

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return filePath;
    }

    private static string GetFilePath(string fileName, string directoryPath)
    {
        string filePath = $"{Path.GetFileNameWithoutExtension(fileName)}{Guid.NewGuid()}.{Path.GetExtension(fileName)}";
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
        return Path.Combine(directoryPath, filePath);
    }
}

using CommonObjects.Results;
using SpoofFileInfo;
using SpoofFileService.Models;

namespace SpoofFileService.Services;

public interface IExtensionService
{
    public Task<Result<Extension>> GetByName(FileExtension2 fileExtension2);
    public Task<Result<Extension>> GetByFile(string filePath);
}

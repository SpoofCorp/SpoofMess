using CommonObjects.Results;
using SpoofFileInfo;
using SpoofFileService.Models;

namespace SpoofFileService.Services;

public interface IExtensionService
{
    public Task<Result<ExtensionDto>> GetByName(FileExtension2 fileExtension2);
    public Task<Result<ExtensionDto>> GetByFile(string filePath);
}

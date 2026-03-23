using CommonObjects.Results;
using SpoofFileParser;
using SpoofFileService.Models;

namespace SpoofFileService.Services;

public interface IExtensionService
{
    public Task<Result<ExtensionDto>> GetByName(FileExtension fileExtension);
    public Task<Result<ExtensionDto>> GetByFile(string filePath);
}

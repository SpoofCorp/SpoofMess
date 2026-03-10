using CommonObjects.Results;
using SpoofFileService.Models;

namespace SpoofFileService.Services;

public interface IExtensionService
{
    public Task<Result<Extension>> GetByName(string name);
    public Task<Result<Extension>> GetByFile(string filePath);
}

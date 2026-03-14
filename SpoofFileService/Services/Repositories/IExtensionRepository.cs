using SpoofFileInfo;
using SpoofFileService.Models;

namespace SpoofFileService.Services.Repositories;

public interface IExtensionRepository
{
    public Task<ExtensionDto?> GetByName(FileExtension2 fileExtension2);
}

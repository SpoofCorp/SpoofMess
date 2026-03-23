using SpoofFileParser;
using SpoofFileService.Models;

namespace SpoofFileService.Services.Repositories;

public interface IExtensionRepository
{
    public Task<ExtensionDto?> GetByName(FileExtension fileExtension);
}

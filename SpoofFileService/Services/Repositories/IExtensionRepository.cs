using SpoofFileInfo;
using SpoofFileService.Models;

namespace SpoofFileService.Services.Repositories;

public interface IExtensionRepository
{
    public Task<Extension?> GetByName(FileExtension2 fileExtension2);
}

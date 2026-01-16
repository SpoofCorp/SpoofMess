using DataSaveHelpers.Services.Repositories;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Repositories;

public interface IStickerPackRepository : IIdentifiedRepository<StickerPack, long>
{
    public Task<StickerPack?> GetWithStickers(long id);
}

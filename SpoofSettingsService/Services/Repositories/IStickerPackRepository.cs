using DataHelpers.Services;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Repositories;

public interface IStickerPackRepository : IBaseRepository<StickerPack, Guid>
{
    public Task<StickerPack?> GetWithStickers(Guid id);
}

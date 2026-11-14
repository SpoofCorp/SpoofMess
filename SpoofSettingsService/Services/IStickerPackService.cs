using CommonObjects.DTO;
using CommonObjects.Requests.Stickers;
using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services;

[Obsolete("Not check user permissions")]
public interface IStickerPackService
{
    public Task<Result<StickerPackDTO>> CreateStickerPackAsync(CreateStickerPackRequest request, Guid userId);
    public Task<Result<StickerPack>> GetStickerPack(GetStickerPackRequest request);
    public Task<Result<StickerPackDTO>> GetStickerPackAsync(GetStickerPackRequest request);
    public Task<Result<StickerPack>> CreateStickerPack(CreateStickerPackRequest request, Guid userId);
}

using CommonObjects.Requests.Stickers;
using CommonObjects.Responses;
using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services;

public interface IStickerPackService
{
    public Task<Result<GetStickerPackResponse>> GetStickerPack(GetStickerPackRequest request);
    public Task<Result<StickerPack>> GetStickerPackAsync(GetStickerPackRequest request);
}

using CommonObjects.Responses;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Setters;

public static class StickerPackSetter
{
    public static GetStickerPackResponse Response(this StickerPack stickerPack) =>
        new()
        {
            FileId = stickerPack.PreviewId,
            Stickers = [.. stickerPack.Stickers.Select(x => x.Set())]
        };
}

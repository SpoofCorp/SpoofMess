using CommonObjects.DTO;
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


    public static StickerPackDTO Set(this StickerPack stickerPack, string AuthorLogin, byte[] token, byte[] id) =>
        new(
            stickerPack.Preview.Set(
                stickerPack.OriginalFileName,
                token,
                id
            ),
            stickerPack.Key,
            stickerPack.Title,
            AuthorLogin,
            stickerPack.Stickers.Select(x => x.Set())
        );
}

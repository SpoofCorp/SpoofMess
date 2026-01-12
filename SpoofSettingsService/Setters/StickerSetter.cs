using CommonObjects.DTO;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Setters;

public static class StickerSetter
{
    public static StickerDTO Set(this Sticker sticker) =>
        new()
        {
            FileId = sticker.FileId,
            Id = sticker.Id,
            Name = sticker.Title,
        };
}

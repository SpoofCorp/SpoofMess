using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Validators;

public interface IStickerPackValidator
{
    public Result IsAvailable(StickerPack? stickerPack);

    public Result IsOwner(StickerPack? stickerPack, Guid userId);
}

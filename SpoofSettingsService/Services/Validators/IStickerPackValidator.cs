using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Validators;

public interface IStickerPackValidator : ISoftDeletableValidator<StickerPack>
{
    public Result IsOwner(StickerPack? stickerPack, Guid userId);
}

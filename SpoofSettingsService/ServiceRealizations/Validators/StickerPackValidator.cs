using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Validators;
using DataSaveHelpers.ServiceRealizations;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class StickerPackValidator : SoftDeletableValidator<StickerPack>, IStickerPackValidator
{
    public Result IsOwner(StickerPack? stickerPack, Guid userId)
    {
        Result result = IsAvailable(stickerPack);
        if(!result.Success)
        return result;

        if (stickerPack!.AuthorId != userId)
            return Result.Forbidden("You is not sticker pack author");

        return Result.OkResult();
    }
}

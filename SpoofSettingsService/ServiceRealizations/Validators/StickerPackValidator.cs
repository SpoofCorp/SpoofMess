using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class StickerPackValidator : IStickerPackValidator
{
    public Result IsAvailable(StickerPack? stickerPack)
    {
        if (stickerPack is null)
            return Result.NotFoundResult("Invalid id");
        if (stickerPack.IsDeleted)
            return Result.BadRequest("Sticker pack has been deleted");

        return Result.SuccessResult();
    }

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

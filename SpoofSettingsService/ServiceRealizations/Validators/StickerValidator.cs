using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class StickerValidator : IStickerValidator
{
    public Result IsNotNullOrNotDeleted(Sticker? obj)
    {
        if (obj is null)
            return Result.NotFoundResult("Invalid id");
        if (obj.IsDeleted)
            return Result.BadRequest("Sticker has been deleted");
        if (obj.FileId is null)
            return Result.BadRequest("Sticker has broken");

        return Result.SuccessResult();
    }

    public bool IsPublic(Sticker? obj)
    {
        throw new NotImplementedException();
    }
}

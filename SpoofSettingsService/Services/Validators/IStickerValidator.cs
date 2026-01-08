using CommonObjects.Results;
using DataHelpers;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Validators;

public interface IStickerValidator
{
    public Result IsNotNullOrNotDeleted(Sticker? obj);

    public bool IsPublic(Sticker? obj);
}

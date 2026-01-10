using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class StickerValidator : SoftDeletableValidator<Sticker>, IStickerValidator;
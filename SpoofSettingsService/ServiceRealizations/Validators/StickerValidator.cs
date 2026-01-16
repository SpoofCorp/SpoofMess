using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Validators;
using DataSaveHelpers.ServiceRealizations;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class StickerValidator : SoftDeletableValidator<Sticker>, IStickerValidator;
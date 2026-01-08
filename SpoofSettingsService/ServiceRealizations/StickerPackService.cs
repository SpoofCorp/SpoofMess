using AdditionalHelpers.Services;
using CommonObjects.Requests.Stickers;
using CommonObjects.Responses;
using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;
using SpoofSettingsService.Setters;

namespace SpoofSettingsService.ServiceRealizations;

public class StickerPackService(ILoggerService loggerService, IStickerPackRepository stickerPackRepository, IStickerPackValidator stickerPackValidator) : IStickerPackService
{
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IStickerPackRepository _stickerPackRepository = stickerPackRepository;
    private readonly IStickerPackValidator _stickerPackValidator = stickerPackValidator;
    public async Task<Result<GetStickerPackResponse>> GetStickerPack(GetStickerPackRequest request)
    {
        try
        {
            Result<StickerPack> result = await GetStickerPackAsync(request);

            if (!result.Success)
                return Result<GetStickerPackResponse>.From(result);

            return Result<GetStickerPackResponse>.OkResult(result.Body!.Response());
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<GetStickerPackResponse>.ErrorResult("Internal server error");
        }
    }

    public async Task<Result<StickerPack>> GetStickerPackAsync(GetStickerPackRequest request)
    {
        try
        {
            StickerPack? stickerPack;
            if(request.WithStickers)
                stickerPack = await _stickerPackRepository.GetWithStickers(request.Id);
            else
                stickerPack = await _stickerPackRepository.GetByIdAsync(request.Id);

            Result result = _stickerPackValidator.IsAvailable(stickerPack);
            if (!result.Success)
                return Result<StickerPack>.From(result);

            return Result<StickerPack>.OkResult(stickerPack!);
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<StickerPack>.ErrorResult("Internal server error");
        }
    }
}

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

public class StickerService(ILoggerService loggerService, IStickerRepository stickerRepository, IStickerPackService stickerPackService, IStickerValidator stickerValidator, IStickerPackValidator stickerPackValidator) : IStickerService
{
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IStickerRepository _stickerRepository = stickerRepository;
    private readonly IStickerPackService _stickerPackService = stickerPackService;
    private readonly IStickerValidator _stickerValidator = stickerValidator;
    private readonly IStickerPackValidator _stickerPackValidator = stickerPackValidator;

    public async Task<Result> CreateAsync(CreateStickerRequest request, Guid userId)
    {
        try
        {
            Result<StickerPack> stickerPack = await _stickerPackService.GetStickerPackAsync(new() { Id = request.StickerPackId });
            if (!stickerPack.Success)
                return Result.From(stickerPack);

            Result result = _stickerPackValidator.IsOwner(stickerPack.Body, userId);
            if (!result.Success)
                return result;

            Sticker sticker = new()
            {
                Id = Guid.CreateVersion7(),
                File = request.FileMetadata.Set(),
                LastModified = DateTime.UtcNow,
                IsDeleted = false,
                StickerPackId = request.StickerPackId,
                Title = request.StickerName,
            };

            await _stickerRepository.AddAsync(sticker);
            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result.ErrorResult("Internal server error");
        }
    }

    public async Task<Result> DeleteAsync(DeleteStickerRequest request, Guid userId)
    {
        try
        {
            Result<StickerPack> stickerPack = await _stickerPackService.GetStickerPackAsync(new() { Id = request.StickerPackId });
            if (!stickerPack.Success)
                return Result.From(stickerPack);

            Result result = _stickerPackValidator.IsOwner(stickerPack.Body, userId);
            if (!result.Success)
                return result;

            return await _stickerRepository.SoftDeleteAsync(request.StickerId) ? Result.OkResult() : Result.BadRequest("Invalid sticker id");
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result.ErrorResult("Internal server error");
        }
    }

    public async Task<Result<GetStickerResponse>> GetAsync(GetStickerRequest request)
    {
        try
        {
            Sticker? sticker = await _stickerRepository.GetByIdAsync(request.StickerId);
            Result result = _stickerValidator.IsAvailable(sticker);
            if (!result.Success)
                return Result<GetStickerResponse>.From(result);

            return Result<GetStickerResponse>.OkResult(new() { FileId = sticker!.FileId });
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<GetStickerResponse>.ErrorResult("Internal server error");
        }
    }
}

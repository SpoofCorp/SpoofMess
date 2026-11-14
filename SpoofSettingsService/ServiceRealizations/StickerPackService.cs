using AdditionalHelpers.Services;
using CommonObjects.DTO;
using CommonObjects.Requests.Stickers;
using CommonObjects.Results;
using SecurityLibrary;
using SecurityLibrary.Tokens;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;
using SpoofSettingsService.Setters;

namespace SpoofSettingsService.ServiceRealizations;

public class StickerPackService(
    ILoggerService loggerService,
    IStickerPackRepository stickerPackRepository,
    IStickerPackValidator stickerPackValidator,
    Hasher hasher,
    IAttachmentAccessTokenService attachmentAccessTokenService,
    IFileTokenService fileTokenService) : IStickerPackService
{
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IStickerPackRepository _stickerPackRepository = stickerPackRepository;
    private readonly IStickerPackValidator _stickerPackValidator = stickerPackValidator;
    private readonly Hasher _hasher = hasher;
    private readonly IAttachmentAccessTokenService _attachmentAccessTokenService = attachmentAccessTokenService;
    private readonly IFileTokenService _fileTokenService = fileTokenService;

    public async Task<Result<StickerPackDTO>> CreateStickerPackAsync(CreateStickerPackRequest request, Guid userId)
    {
        try
        {
            Result<StickerPack> result = await CreateStickerPack(request, userId);
            if (result)
            {
                result.Body!.Set(
                    result.Body!.Author.Login,
                    _hasher.GetKey(result.Body.PreviewId.ToByteArray()),
                    _attachmentAccessTokenService.CreateToken(result.Body.PreviewId)
                );
            }
            return Result<StickerPackDTO>.From(result);
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<StickerPackDTO>.ErrorResult("Internal server error");
        }
    }

    public async Task<Result<StickerPack>> CreateStickerPack(CreateStickerPackRequest request, Guid userId)
    {
        try
        {
            if (!_fileTokenService.IsValid(request.FileMetadata.Token, userId, out Guid previewId))
                return Result<StickerPack>.Forbidden("Unvalid access token");

            StickerPack stickerPack = new()
            {
                AuthorId = userId,
                Key = request.Key,
                Title = request.Name,
                PreviewId = previewId,
                OriginalFileName = request.FileMetadata.OriginalName
            };
            await _stickerPackRepository.AddAsync(stickerPack);

            return Result<StickerPack>.OkResult(stickerPack);
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<StickerPack>.ErrorResult("Internal server error");
        }
    }

    public async Task<Result<StickerPackDTO>> GetStickerPackAsync(GetStickerPackRequest request)
    {
        try
        {
            Result<StickerPack> result = await GetStickerPack(request);

            if (!result)
                return Result<StickerPackDTO>.From(result);

            return Result<StickerPackDTO>.OkResult(result.Body!.Set(
                    result.Body!.Author.Login,
                    _hasher.GetKey(result.Body.PreviewId.ToByteArray()),
                    _attachmentAccessTokenService.CreateToken(result.Body.PreviewId)
                ));
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<StickerPackDTO>.ErrorResult("Internal server error");
        }
    }

    public async Task<Result<StickerPack>> GetStickerPack(GetStickerPackRequest request)
    {
        try
        {
            StickerPack? stickerPack;
            if (request.WithStickers)
                stickerPack = await _stickerPackRepository.GetWithStickers(request.Key);
            else
                stickerPack = await _stickerPackRepository.GetByKeyAsync(request.Key);

            Result result = _stickerPackValidator.IsAvailable(stickerPack);
            if (!result)
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

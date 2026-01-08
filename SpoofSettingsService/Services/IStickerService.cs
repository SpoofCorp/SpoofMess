using CommonObjects.Requests.Stickers;
using CommonObjects.Responses;
using CommonObjects.Results;

namespace SpoofSettingsService.Services;

public interface IStickerService
{
    public Task<Result> CreateAsync(CreateStickerRequest request, Guid userId);

    public Task<Result> DeleteAsync(DeleteStickerRequest request, Guid userId);

    public Task<Result<GetStickerResponse>> GetAsync(GetStickerRequest request);
}

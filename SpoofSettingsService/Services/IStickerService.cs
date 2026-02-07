using CommonObjects.Requests.Stickers;
using CommonObjects.Responses;
using CommonObjects.Results;

namespace SpoofSettingsService.Services;

[Obsolete("Not check user permissions")]
public interface IStickerService
{
    public Task<Result> CreateAsync(CreateStickerRequest request, Guid userId);

    public Task<Result> DeleteAsync(DeleteStickerRequest request, Guid userId);

    public Task<Result<GetStickerResponse>> GetAsync(GetStickerRequest request);
}

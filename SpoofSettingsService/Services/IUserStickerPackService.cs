using CommonObjects.DTO;
using CommonObjects.Results;

namespace SpoofSettingsService.Services;

public interface IUserStickerPackService
{
    public Task<Result<IEnumerable<UserStickerPackDTO>>> GetUserStickerPacks(Guid userId);

}

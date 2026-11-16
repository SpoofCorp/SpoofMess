using CommonObjects.DTO;
using CommonObjects.Results;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations;

public class UserStickerPackService(
        IUserStickerPackRepository userStickerPackRepository
    ) : IUserStickerPackService
{
    private readonly IUserStickerPackRepository _userStickerPackRepository = userStickerPackRepository;

    public async Task<Result<IEnumerable<UserStickerPackDTO>>> GetUserStickerPacks(Guid userId)
    {
        try
        {
            IEnumerable<UserStickerPackDTO> result = await _userStickerPackRepository.GetUserStickerPacks(userId);
            return result.Any() ? Result<IEnumerable<UserStickerPackDTO>>.OkResult(result) : Result<IEnumerable<UserStickerPackDTO>>.NoContent();
        }
        catch(Exception ex)
        {
            return Result<IEnumerable<UserStickerPackDTO>>.From(ex);
        }
    }
}

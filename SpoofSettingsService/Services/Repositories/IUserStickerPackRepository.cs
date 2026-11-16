using CommonObjects.DTO;
using DataSaveHelpers.Services.Repositories;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Repositories;

public interface IUserStickerPackRepository : IDoubleIdentifiedRepository<UserStickerPack, Guid, long>
{
    public Task<IEnumerable<UserStickerPackDTO>> GetUserStickerPacks(Guid userId);
}

using DataSaveHelpers.Services.Repositories;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Repositories;

public interface IStickerRepository : ISoftDeletableIdentifiedRepository<Sticker, Guid>
{

    public Task<bool> SoftDeleteAsync(string key);
}

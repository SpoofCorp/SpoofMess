using DataSaveHelpers.Services.Repositories;
using SpoofSettingsService.Models.Enums;

namespace SpoofSettingsService.Services.Repositories;

public interface IRuleRepository
{
    public Task<HasPermission?> HasPermission(Guid userId, Guid chatId, short permissionId);
}

using CommonObjects.Results;
using SpoofSettingsService.Models.Enums;

namespace SpoofSettingsService.Services;

public interface IRuleService
{
    public Task<Result<HasPermission>> HasPermissionAsync(Guid userId, Guid chatId, Permissions permission);
}

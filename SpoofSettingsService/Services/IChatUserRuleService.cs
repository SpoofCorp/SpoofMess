using CommonObjects.Results;
using SpoofSettingsService.Models.Enums;

namespace SpoofSettingsService.Services;

public interface IChatUserRuleService
{
    public Task<Result> HasPermission(Guid userId, Guid chatId, Permissions permission);
}

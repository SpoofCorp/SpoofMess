using CommonObjects.Results;
using RuleRoleHelper;

namespace SpoofSettingsService.Services;

public interface IChatUserRuleService
{
    public Task<Result> HasPermission(Guid userId, Guid chatId, Permissions permission);
}

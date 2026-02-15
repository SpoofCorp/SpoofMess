using CommonObjects.Results;
using CommunicationLibrary.Communication;
using RuleRoleHelper;

namespace SpoofSettingsService.Services;

public interface IRuleService
{
    public Task<Result<HasPermission>> HasPermissionAsync(Guid userId, Guid chatId, Permissions permission);
    public Task<Result<Rule[]>> ChatUserRulesForSMS(Guid chatId, Guid userId);
}

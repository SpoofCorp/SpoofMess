using CommunicationLibrary.Communication;
using RuleRoleHelper;

namespace SpoofSettingsService.Services.Repositories;

public interface IRuleRepository
{
    public Task<HasPermission?> HasPermission(Guid userId, Guid chatId, short permissionId);
    public Task<Rule[]?> ChatUserRules(Guid chatId, Guid userId, short[] masks);
}

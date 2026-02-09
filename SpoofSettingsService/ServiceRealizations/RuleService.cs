using CommonObjects.Results;
using SpoofSettingsService.Models.Enums;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations;

public class RuleService(IRuleRepository ruleRepository) : IRuleService
{
    private readonly IRuleRepository _ruleRepository = ruleRepository;

    async Task<Result<HasPermission>> IRuleService.HasPermissionAsync(Guid userId, Guid chatId, Permissions permission)
    {
        HasPermission? hasPermission = await _ruleRepository.HasPermission(userId, chatId, (short)permission);
        if (hasPermission is null)
            return Result<HasPermission>.ErrorResult("DataBase error");
        if(hasPermission is HasPermission.Allow)
            return Result<HasPermission>.OkResult(hasPermission.Value);
        return Result<HasPermission>.Forbidden(hasPermission is HasPermission.NotSet ? "Not permitted" : "Access Denied");
    }
}

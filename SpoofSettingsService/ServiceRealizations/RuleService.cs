using CommonObjects.Results;
using CommunicationLibrary.Communication;
using RuleRoleHelper;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations;

public class RuleService(IRuleRepository ruleRepository, IRuleValidator ruleValidator) : IRuleService
{
    private readonly IRuleRepository _ruleRepository = ruleRepository;
    private readonly IRuleValidator _ruleValidator = ruleValidator;

    public async Task<Result<HasPermission>> HasPermissionAsync(Guid userId, Guid chatId, Permissions permission)
    {
        HasPermission? hasPermission = await _ruleRepository.HasPermission(userId, chatId, (short)permission);
        if (hasPermission is null)
            return Result<HasPermission>.ErrorResult("DataBase error");
        if (hasPermission is HasPermission.Allow)
            return Result<HasPermission>.OkResult(hasPermission.Value);
        return Result<HasPermission>.Forbidden(hasPermission is HasPermission.NotSet ? "Not permitted" : "Access Denied");
    }

    public async Task<Result<Rule[]>> ChatUserRulesForSMS(Guid chatId, Guid userId) =>
        await ChatUserRules(
            chatId,
            userId,
            [
                (short)Permissions.SendTexts,
                (short)Permissions.SendAudios,
                (short)Permissions.SendVideos,
                (short)Permissions.SendFiles,
                (short)Permissions.SendEmoji,
                (short)Permissions.SendSticker,
                (short)Permissions.SendVoiceMessage,
                (short)Permissions.SendVideoMessage,
                (short)Permissions.ShareMessage,
                (short)Permissions.DeleteMessage,
                (short)Permissions.EditMessage
            ]);

    private async Task<Result<Rule[]>> ChatUserRules(Guid chatId, Guid userId, short[] mask)
    {
        Rule[]? rules = await _ruleRepository.ChatUserRules(
            chatId,
            userId,
            mask
            );
        Result result = _ruleValidator.IsAvailableCollection(rules);
        if (result.Success)
            return Result<Rule[]>.OkResult(rules!);
        return Result<Rule[]>.From(result);
    }
}

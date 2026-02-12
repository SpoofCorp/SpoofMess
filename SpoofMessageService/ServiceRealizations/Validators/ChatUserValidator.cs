using CommonObjects.Results;
using DataSaveHelpers.ServiceRealizations;
using RuleRoleHelper;
using RuleRoleHelper.Services;
using SpoofMessageService.Models;
using SpoofMessageService.Models.Enums;
using SpoofMessageService.Services.Validators;

namespace SpoofMessageService.ServiceRealizations.Validators;

public class ChatUserValidator(IRuleService ruleService) : SoftDeletableValidator<ChatUser>, IChatUserValidator
{
    private readonly IRuleService ruleService = ruleService;

    public Result IsAvailableAndHasPermission(ChatUser? chatUser, Rules rule)
    {
        Result result = IsAvailable(chatUser);
        if (result.Success)
        {
            HasPermission hasPermission = ruleService.HasPermission(chatUser!.Rules, (long)rule);
            if (hasPermission == HasPermission.Allow)
                return Result.OkResult();
            else
                return Result.Forbidden($"User doesn't have permission to {rule}");
        }

        return result;
    }
}

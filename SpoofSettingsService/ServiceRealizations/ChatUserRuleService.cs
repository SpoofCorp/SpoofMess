using AdditionalHelpers.Services;
using CommonObjects.Requests.ChatUsers;
using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations;

public class ChatUserRuleService(ILoggerService loggerService, IChatUserRuleRepository chatUserRuleRepository, IChatUserService chatUserService)
{
    private readonly IChatUserService _chatUserService = chatUserService;
    private readonly IChatUserRuleRepository _chatUserRuleRepository = chatUserRuleRepository;
    private readonly ILoggerService _loggerService = loggerService;

    public async Task<Result> ChangeMemberRights(ChangeRulesRequest request)
    {
        try
        {
            Result<ChatUser>? chatUserResult = await _chatUserService.Get(new(request.ChatId, request.UserId));
            if (!chatUserResult.Success) return Result.From(chatUserResult);

            ChatUserRule? rule = chatUserResult.Body!.ChatUserRules.FirstOrDefault(x => x.PermissionId == request.RuleId);
            if (rule is null)
            {
                rule = new()
                {
                    Key1 = request.ChatId,
                    Key2 = request.UserId,
                    IsPermission = request.IsPermission,
                    PermissionId = request.RuleId,
                };
                await _chatUserRuleRepository.AddAsync(rule);
            }
            else if (rule.IsPermission != request.IsPermission)
            {
                rule.IsPermission = request.IsPermission;
                await _chatUserRuleRepository.UpdateAsync(rule);
            }
            else
                return Result.BadRequest("Rule already setted");

            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("DataBase error");
        }
    }
}

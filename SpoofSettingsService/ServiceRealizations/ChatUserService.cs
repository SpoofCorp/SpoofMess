using AdditionalHelpers.Services;
using CommonObjects.Requests.ChatUsers;
using CommonObjects.Requests.Members;
using CommonObjects.Results;
using CommunicationLibrary.Communication;
using RuleRoleHelper;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.MessageBrokers;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations;

public class ChatUserService(
    ILoggerService loggerService,
    IChatUserValidator chatUserValidator,
    IRuleService ruleService,
    IChatService chatService,
    IUserService userService,
    IChatUserRepository chatUserRepository,
    IChatUserPublisherService chatUserPublisherService
    ) : IChatUserService
{
    private readonly IUserService _userService = userService;
    private readonly IChatUserValidator _chatUserValidator = chatUserValidator;
    private readonly IChatUserRepository _chatUserRepository = chatUserRepository;
    private readonly IRuleService _ruleService = ruleService;
    private readonly IChatService _chatService = chatService;
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IChatUserPublisherService _chatUserPublisherService = chatUserPublisherService;

    public async Task<Result> Join(JoinToChatRequest request, Guid userId)
    {
        try
        { 
            Task<Result<Chat>> chatResult = _chatService.Get(request.ChatId);
            Task<Result<User>> memberResult = _userService.Get(userId);

            await Task.WhenAll(chatResult, memberResult);

            if (!chatResult.Result.Success)
                return Result.From(chatResult.Result);

            if (!memberResult.Result.Success)
                return Result.From(memberResult.Result);

            ChatUser newMember = new()
            {
                Key1 = chatResult.Result.Body!.Id,
                Key2 = memberResult.Result.Body!.Id,
                JoinedAt = DateTime.UtcNow,
            };
            await _chatUserRepository.AddAsync(newMember);
            _ = Task.Run(async () =>
            {
                Result<Rule[]> rulesResult = await _ruleService.ChatUserRulesForSMS(newMember.Key1, newMember.Key2);
                if (rulesResult.Success)
                    await _chatUserPublisherService.Create(newMember, rulesResult.Body!);
                else
                    _loggerService.Error($"Can't find rules for: {newMember.Key1}, {newMember.Key2}\n{rulesResult.Error}");
            });
            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("DataBase error");
        }
    }

    public async Task<Result> Add(AddMemberRequest request, Guid userId)
    {
        try
        {
            Result<HasPermission> result = await _ruleService.HasPermissionAsync(request.ChatId, userId, Permissions.Inviting);
            if (!result.Success)
                return Result.From(result);

            Result<User>? memberResult = await _userService.Get(request.MemberId);
            if (!memberResult.Success) return Result.From(memberResult);

            ChatUser newMember = new()
            {
                Key1 = request.ChatId,
                Key2 = memberResult.Body!.Id,
                JoinedAt = DateTime.UtcNow,
            };
            await _chatUserRepository.AddAsync(newMember);
            _ = Task.Run(async () =>
            {
                Result<Rule[]> rulesResult = await _ruleService.ChatUserRulesForSMS(newMember.Key1, newMember.Key2);
                if (rulesResult.Success)
                    await _chatUserPublisherService.Create(newMember, rulesResult.Body!);
                else
                    _loggerService.Error($"Can't find rules for: {newMember.Key1}, {newMember.Key2}\n{rulesResult.Error}");
            });
            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("DataBase error");
        }
    }

    public async Task<Result<ChatUser>> Get(GetChatUserRequest request)
    {
        try
        {
            ChatUser? chatUser = await _chatUserRepository.GetWithRules(request.ChatId, request.UserId);
            Result result = _chatUserValidator.IsAvailable(chatUser);
            if (!result.Success) return Result<ChatUser>.From(result);

            return Result<ChatUser>.OkResult(chatUser!);
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result<ChatUser>.ErrorResult("DataBase error");
        }
    }

    public async Task<Result> Remove(DeleteMemberRequest request, Guid userId)
    {
        try
        {
            Result<ChatWithOwner> result = await _chatService.GetChatWithOwner(userId, request.ChatId);
            if (!result.Success)
                return Result.From(result);

            await _chatUserRepository.SoftDeleteAsync(request.ChatId, request.ChatId);
            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("DataBase error");
        }
    }
}

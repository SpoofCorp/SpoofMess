using AdditionalHelpers.Services;
using CommonObjects.Requests;
using CommonObjects.Requests.Changes;
using CommonObjects.Results;
using RuleRoleHelper;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.MessageBrokers;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;
using SpoofSettingsService.Setters;

namespace SpoofSettingsService.ServiceRealizations;

public class ChatService(
    IChatRepository chatRepository,
    IChatTypeService chatTypeService,
    IChatPublisherService chatPublisherService,
    IChatValidator chatValidator,
    IUserService userService,
    IRuleService ruleService,
    ILoggerService loggerService
    ) : IChatService
{
    private readonly IChatValidator _chatValidator = chatValidator;
    private readonly IUserService _userService = userService;
    private readonly IChatRepository _chatRepository = chatRepository;
    private readonly IChatTypeService _chatTypeService = chatTypeService;
    private readonly IRuleService _ruleService = ruleService;
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IChatPublisherService _chatPublisherService = chatPublisherService;

    public async ValueTask<Result> ChangeSettings(ChangeChatSettingsRequest request, Guid userId)
    {
        Chat? chat = await _chatRepository.GetByIdAsync(request.Id);
        Result result = _chatValidator.IsAvailable(chat);
        if (!result.Success)
            return result;
        Result<HasPermission> permissionResult = await _ruleService.HasPermissionAsync(userId, request.Id, Permissions.ChangeSettings);
        if (!permissionResult.Success)
            return Result.From(permissionResult);

        chat!.Set(request);
        await _chatRepository.UpdateAsync(chat!);
        return Result.OkResult();
    }


    public async ValueTask<Result> CreateChat(CreateChatRequest request, Guid userId)
    {
        Result<User> userResult = await _userService.Get(userId);
        if (!userResult.Success) return Result.From(userResult);

        Result<ChatType> chatTypeResult = await _chatTypeService.Get(request.ChatTypeId);
        if (!chatTypeResult.Success) return Result.From(chatTypeResult);

        Chat? repetition = await _chatRepository.GetByUniqueName(request.UniqueName);
        Result result = _chatValidator.ValidateHasChatUniqueName(repetition);
        if (!result.Success) return result;

        DateTime now = DateTime.UtcNow;
        Chat newChat = new(
            Guid.CreateVersion7(),
            request.ChatTypeId,
            userResult.Body!.Id,
            request.ChatName,
            request.UniqueName,
            now,
            now);

        await _chatRepository.Change(newChat, repetition);
        await _chatPublisherService.Publish(new(newChat.Id, null, newChat.ChatUniqueName, newChat.ChatName));
        return Result.OkResult();
    }

    public async ValueTask<Result> DeleteChat(Guid chatId, Guid userId)
    {
        Result<ChatWithOwner> result = await GetChatWithOwner(userId, chatId);
        if (!result.Success)
            return Result.From(result);

        await _chatRepository.SoftDeleteAsync(result.Body.Chat!);
        return Result.OkResult();
    }

    public async Task<Result<Chat>> Get(Guid chatId)
    {
        try
        {
            Chat? chat = await _chatRepository.GetByIdAsync(chatId);
            Result result = _chatValidator.IsAvailable(chat);
            if(!result.Success)
                return Result<Chat>.From(result);

            return Result<Chat>.OkResult(chat!);
        }
        catch (Exception ex)
        {
            loggerService.Error("Database error", ex);
            return Result<Chat>.ErrorResult("Database error", 400);
        }
    }

    public async Task<Result<ChatWithOwner>> GetChatWithOwner(Guid userId, Guid chatId)
    {
        Task<Result<User>> userResult = _userService.Get(userId);
        Task<Chat?> chatResult = _chatRepository.GetByIdAsync(chatId);
        await Task.WhenAll(userResult, chatResult);

        if (!userResult.Result.Success)
            return Result<ChatWithOwner>.From(userResult.Result);

        Result result = _chatValidator.ValidateChatAndOwner(chatResult.Result, userId);
        if (!result.Success)
            return Result<ChatWithOwner>.From(result);

        return Result<ChatWithOwner>.OkResult(new(userResult.Result.Body!, chatResult.Result!));
    }
}
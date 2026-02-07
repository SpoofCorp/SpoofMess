using AdditionalHelpers.Services;
using CommonObjects.Requests.ChatUsers;
using CommonObjects.Requests.Members;
using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations;

public class ChatUserService(ILoggerService loggerService, IChatUserValidator chatUserValidator, IChatService chatService, IUserService userService, IChatUserRepository chatUserRepository) : IChatUserService
{
    private readonly IUserService _userService = userService;
    private readonly IChatUserValidator _chatUserValidator = chatUserValidator;
    private readonly IChatUserRepository _chatUserRepository = chatUserRepository;
    private readonly IChatService _chatService = chatService;
    private readonly ILoggerService _loggerService = loggerService;

    public async Task<Result> Add(AddMemberRequest request, Guid userId)
    {
        try
        {
            Result<ChatWithOwner> result = await _chatService.GetChatWithOwner(userId, request.ChatId);
            if (!result.Success)
                return Result.From(result);

            Result<User>? memberResult = await _userService.Get(request.MemberId);
            if (!memberResult.Success) return Result.From(memberResult);

            ChatUser newMember = new()
            {
                Key1 = result.Body.User!.Id,
                Key2 = memberResult.Body!.Id,
                JoinedAt = DateTime.UtcNow,
            };

            await _chatUserRepository.AddAsync(newMember);
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

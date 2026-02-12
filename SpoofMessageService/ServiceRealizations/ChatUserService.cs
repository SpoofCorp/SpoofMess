using AdditionalHelpers.Services;
using CommonObjects.Results;
using SpoofMessageService.Models;
using SpoofMessageService.Models.Enums;
using SpoofMessageService.Services;
using SpoofMessageService.Services.Repositories;
using SpoofMessageService.Services.Validators;

namespace SpoofMessageService.ServiceRealizations;

public class ChatUserService(IChatUserRepository chatUserRepository, IChatUserValidator chatUserValidator, ILoggerService loggerService) : IChatUserService
{
    private readonly IChatUserRepository _chatUserRepository = chatUserRepository;
    private readonly IChatUserValidator _chatUserValidator = chatUserValidator;
    private readonly ILoggerService _loggerService = loggerService;

    public async Task<Result<ChatUser>> Get(Guid chatId, Guid userId)
    {
        try
        {
            ChatUser? chatUser = await _chatUserRepository.GetByIdAsync(chatId, userId);
            Result result = _chatUserValidator.IsAvailable(chatUser);
            if (!result.Success)
                return Result<ChatUser>.From(result);

            return Result<ChatUser>.OkResult(chatUser!);
        }
        catch(Exception ex)
        {
            _loggerService.Error($"An error occurred while getting chat users: {ex.Message}");
            return Result<ChatUser>.ErrorResult("An error occurred while getting chat users.");
        }
    }

    public async Task<Result<ChatUser>> GetAndCheckPermission(Guid chatId, Guid userId, Rules rule)
    {
        Result<ChatUser> chatUserResult = await Get(chatId, userId);
        if(chatUserResult.Success)
        {
            Result result = _chatUserValidator.HasPermission(chatUserResult.Body!, rule);
            if (result.Success)
                return chatUserResult;
            return Result<ChatUser>.From(result);
        }
        return chatUserResult;
    }
}

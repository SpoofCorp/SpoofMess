using AdditionalHelpers.Services;
using CommonObjects.Requests.Messages;
using CommonObjects.Results;
using SpoofMessageService.Models;
using SpoofMessageService.Models.Enums;
using SpoofMessageService.Services;
using SpoofMessageService.Services.Repositories;
using SpoofMessageService.Services.Setters;
using SpoofMessageService.Services.Validators;

namespace SpoofMessageService.ServiceRealizations;

public class MessageService(ILoggerService loggerService, IMessageRepository messageRepository, IMessageValidator messageValidator, IChatUserService chatUserService) : IMessageService
{
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly IMessageValidator _messageValidator = messageValidator;
    private readonly IChatUserService _chatUserService = chatUserService;

    public async Task<Result> DeleteMessage(DeleteMessageRequest request, Guid userId)
    {
        try
        {
            Task<Message?> messageTask = Task.Run(() => _messageRepository.GetByIdAsync(request.Id));
            Task<Result<ChatUser>> resultTask = Task.Run(() => _chatUserService.GetAndCheckPermission(request.ChatId, userId, Rules.DeleteMessage));
            await Task.WhenAll(messageTask, resultTask);

            if (!resultTask.Result.Success)
                return Result.From(resultTask.Result);

            Result result = _messageValidator.IsAvailableAndOwner(messageTask.Result, request.ChatId);
            if(!result.Success)
                return result;

            await _messageRepository.DeleteAsync(messageTask.Result!);
            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("Internal server error");
        }
    }

    public async Task<Result> EditMessage(EditMessageRequest request, Guid userId)
    {
        try
        {
            Task<Message?> messageTask = Task.Run(() => _messageRepository.GetByIdAsync(request.Id));
            Task<Result<ChatUser>> resultTask = Task.Run(() => _chatUserService.GetAndCheckPermission(request.ChatId, userId, Rules.EditMessage));
            await Task.WhenAll(messageTask, resultTask);

            if (!resultTask.Result.Success)
                return Result.From(resultTask.Result);
            Result result = _messageValidator.IsAvailableAndOwner(messageTask.Result, request.ChatId);
            if (!result.Success)
                return result;

            messageTask.Result!.Set(request, OperationsStatus.Pending);
            await _messageRepository.UpdateAsync(messageTask.Result!);

            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("Internal server error");
        }
    }

    public async Task<Result> SendMessage(CreateMessageRequest request, Guid userId)
    {
        try
        {
            Result<ChatUser> chatUserResult = await _chatUserService.GetAndCheckPermission(request.ChatId, userId, Rules.EditMessage);
            if(!chatUserResult.Success)
                return Result.From(chatUserResult);


            Message message = request.Set(userId, OperationsStatus.Pending);
            await _messageRepository.AddAsync(message);

            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("Internal server error");
        }
    }
}

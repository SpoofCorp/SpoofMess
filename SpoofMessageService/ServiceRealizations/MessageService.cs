using AdditionalHelpers.Services;
using CommonObjects.Requests.Messages;
using CommonObjects.Results;
using SpoofMessageService.Models;
using SpoofMessageService.Services;
using SpoofMessageService.Services.Setters;
using SpoofMessageService.Services.Validators;

namespace SpoofMessageService.ServiceRealizations;

public class MessageService(ILoggerService loggerService, IMessageRepository messageRepository, IMessageValidator messageValidator) : IMessageService
{
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly IMessageValidator _messageValidator = messageValidator;

    public async Task<Result> DeleteMessage(DeleteMessageRequest request, Guid userId)
    {
        try
        {
            Message? message = await _messageRepository.GetByIdAsync(request.Id);
            Result result = _messageValidator.IsAvailableAndOwner(message, userId);
            if(!result.Success)
                return result;

            await _messageRepository.DeleteAsync(message!);
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
            Message? message = await _messageRepository.GetByIdAsync(request.Id);
            Result result = _messageValidator.IsAvailableAndOwner(message, userId);
            if (!result.Success)
                return result;

            message!.Set(request, OperationsStatus.Pending);

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

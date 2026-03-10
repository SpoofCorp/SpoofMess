using AdditionalHelpers.Services;
using CommonObjects.DTO;
using CommonObjects.Requests.Messages;
using CommonObjects.Results;
using SecurityLibrary.Tokens;
using SpoofMessageService.Models;
using SpoofMessageService.Models.Enums;
using SpoofMessageService.Services;
using SpoofMessageService.Services.Repositories;
using SpoofMessageService.Services.Setters;
using SpoofMessageService.Services.Validators;

namespace SpoofMessageService.ServiceRealizations;

public class MessageService(
        ILoggerService loggerService,
        IMessageRepository messageRepository,
        IMessageValidator messageValidator,
        IChatUserService chatUserService,
        IFileTokenService fileTokenService,
        IFileMetadatumService fileMetadatumService
    ) : IMessageService
{
    private readonly IFileMetadatumService _fileMetadatumService = fileMetadatumService;
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly IMessageValidator _messageValidator = messageValidator;
    private readonly IChatUserService _chatUserService = chatUserService;
    private readonly IFileTokenService _fileTokenService = fileTokenService;

    public async Task<Result> DeleteMessage(
            DeleteMessageRequest request,
            Guid userId
        )
    {
        try
        {
            Task<Message?> messageTask = Task.Run(() => _messageRepository.GetByIdAsync(request.Id));
            Task<Result<ChatUser>> resultTask = Task.Run(() => _chatUserService.GetAndCheckPermission(
                    request.ChatId,
                    userId,
                    Rules.DeleteMessage
                ));
            await Task.WhenAll(messageTask, resultTask);

            if (!resultTask.Result.Success)
                return Result.From(resultTask.Result);

            Result result = _messageValidator.IsAvailableAndOwner(
                    messageTask.Result,
                    request.ChatId
                );
            if (!result.Success)
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

    public async Task<Result> EditMessage(
            EditMessageRequest request,
            Guid userId
        )
    {
        try
        {
            Task<Message?> messageTask = Task.Run(() => _messageRepository.GetByIdAsync(request.Id));
            Task<Result<ChatUser>> resultTask = Task.Run(() => _chatUserService.GetAndCheckPermission(
                    request.ChatId,
                    userId,
                    Rules.EditMessage
                ));
            await Task.WhenAll(messageTask, resultTask);

            if (!resultTask.Result.Success)
                return Result.From(resultTask.Result);
            Result result = _messageValidator.IsAvailableAndOwner(
                    messageTask.Result,
                    request.ChatId
                );
            if (!result.Success)
                return result;

            messageTask.Result!.Set(
                    request
                );

            await _messageRepository.UpdateAsync(messageTask.Result!);

            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("Internal server error");
        }
    }

    public async Task<Result<MessageDTO>> SendMessage(
            CreateMessageRequest request,
            Guid userId
        )
    {
        try
        {
            Result<ChatUser> chatUserResult = await _chatUserService.GetAndCheckPermission(
                    request.ChatId,
                    userId,
                    Rules.SendTexts
                );
            if (!chatUserResult.Success)
                return Result<MessageDTO>.From(chatUserResult);


            Message message = request.Set(
                    userId
                );
            CancellationTokenSource tokenSource = new();
            await Parallel.ForEachAsync(request.Attachments, async (attachment, cancellationToken) =>
            {
                if (!_fileTokenService.IsValid(attachment.Token, userId, out Guid fileId))
                    tokenSource.Cancel();
                Result<FileMetadatum> result = await _fileMetadatumService.Get(fileId);
                if (!result.Success)
                    tokenSource.Cancel();
                message.Attachments.Add(attachment.Set(fileId));
            });
            await _messageRepository.AddAsync(message);
            message.User = chatUserResult.Body!.User;
            return Result<MessageDTO>.OkResult(message.Set([.. message.Attachments.Select(x => _fileTokenService.CreateToken(userId, x.FileMetadata.Id))]));
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result<MessageDTO>.ErrorResult("Internal server error");
        }
    }

    public async Task<Result<List<MessageDTO>>> GetMessagesAfterDate(
            Guid chatId,
            Guid userId,
            DateTime date,
            int take = 50
        )
    {
        try
        {
            Result<ChatUser> resultCHatUser = await _chatUserService.GetAndCheckPermission(
                    chatId,
                    userId,
                    Rules.DeleteMessage
                );
            if (!resultCHatUser.Success)
                return Result<List<MessageDTO>>.From(resultCHatUser);

            return Result<List<MessageDTO>>.OkResult(
                [.. (await _messageRepository.GetMessagesAfterDate(
                    chatId,
                    date,
                    take
                    )).Select(x => x.Set([..x.Attachments.Select(x => _fileTokenService.CreateToken(userId, x.FileMetadata.Id))]))]
                );
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result<List<MessageDTO>>.ErrorResult("Internal server error");
        }
    }

    public async Task<Result<List<MessageDTO>>> GetMessagesBeforeDate(
            Guid chatId,
            Guid userId,
            DateTime date,
            int take = 50
        )
    {
        try
        {
            Result<ChatUser> resultCHatUser = await _chatUserService.GetAndCheckPermission(
                    chatId,
                    userId,
                    Rules.DeleteMessage
                );
            if (!resultCHatUser.Success)
                return Result<List<MessageDTO>>.From(resultCHatUser);

            return Result<List<MessageDTO>>.OkResult(
                [.. (await _messageRepository.GetMessagesBeforeDate(
                    chatId,
                    date,
                    take
                    )).Select(x => x.Set([..x.Attachments.Select(x => _fileTokenService.CreateToken(userId, x.FileMetadata.Id))]))]
                );
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result<List<MessageDTO>>.ErrorResult("Internal server error");
        }
    }

    public async Task<Result<List<MessageDTO>>> GetSkippedMessages(
            Guid userId,
            DateTime after,
            int take = 50
        )
    {
        try
        {
            List<Message> messages = await _messageRepository.GetMessageSinceDate(userId, after, take);
            Result result = _messageValidator.IsAvailableCollection(messages);
            if (!result.Success)
                return Result<List<MessageDTO>>.From(result);

            return Result<List<MessageDTO>>.OkResult(
                [.. messages.Select(x => x.Set([.. x.Attachments.Select(x => _fileTokenService.CreateToken(userId, x.FileMetadata.Id))]))]
                );
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result<List<MessageDTO>>.ErrorResult("Internal server error");
        }
    }
}

using CommonObjects.Requests.Messages;
using CommonObjects.Results;
using SpoofMessageService.Models;

namespace SpoofMessageService.Services;

public interface IMessageService
{
    [Obsolete("Need check permissions for message content")]
    public Task<Result> SendMessage(
        CreateMessageRequest request, 
        Guid userId
    );
    public Task<Result> DeleteMessage(
        DeleteMessageRequest request,
        Guid userId
    );


    [Obsolete("Need check permissions for message content")]
    public Task<Result> EditMessage(
        EditMessageRequest request, 
        Guid userId
    );

    public Task<Result<List<Message>>> GetMessagesAfterDate(
        Guid chatId,
        Guid userId,
        DateTime date,
        int take = 50
    );

    public Task<Result<List<Message>>> GetMessagesBeforeDate(
        Guid chatId,
        Guid userId,
        DateTime date,
        int take = 50
    );
}

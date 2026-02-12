using CommonObjects.Requests.Messages;
using CommonObjects.Results;

namespace SpoofMessageService.Services;

public interface IMessageService
{
    [Obsolete("Need check permissions for message content")]
    public Task<Result> SendMessage(CreateMessageRequest request, Guid userId);

    [Obsolete("Need check permissions for message content")]
    public Task<Result> DeleteMessage(DeleteMessageRequest request, Guid userId);

    public Task<Result> EditMessage(EditMessageRequest request, Guid userId);
}

using CommonObjects.Requests;
using CommonObjects.Results;

namespace SpoofMessageService.Services;

public interface IMessageService
{
    public Task<Result> SendMessage(CreateMessageRequest request);

    public Task<Result> DeleteMessage(DeleteMessageRequest request);

    public Task<Result> EditMessage(EditMessageRequest request);
}

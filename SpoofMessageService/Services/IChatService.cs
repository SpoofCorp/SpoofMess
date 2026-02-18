using CommonObjects.Results;
using CommunicationLibrary.Communication;

namespace SpoofMessageService.Services;

public interface IChatService
{
    public Task<Result> Create(CreateChat createUser);

    public Task<Result> Update();

    public Task<Result> Delete();
}

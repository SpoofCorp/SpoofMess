using CommonObjects.Results;
using CommunicationLibrary.Communication;

namespace SpoofMessageService.Services;

public interface IUserService
{
    public Task<Result> Create(CreateUser createUser);

    public Task<Result> Update();

    public Task<Result> Delete();
}

using CommonObjects.Results;
using CommunicationLibrary.Communication;

namespace SpoofMessageService.Services;

public interface IUserAvatarService
{
    public Task<Result> Create(CreateUserAvatar createUserAvatar);
}

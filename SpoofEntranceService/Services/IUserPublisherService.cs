using CommunicationLibrary.Communication;

namespace SpoofEntranceService.Services;

public interface IUserPublisherService
{
    public Task Create(CreateUser createUser);
}

using CommunicationLibrary.Services;
using SpoofEntranceService.Models;

namespace SpoofEntranceService.Services;

public interface IUserPublisherService : IPublisherService
{
    public Task PublishUser(UserEntry user);
}

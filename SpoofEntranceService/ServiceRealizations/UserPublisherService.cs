using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofEntranceService.Services;

namespace SpoofEntranceService.ServiceRealizations;

public class UserPublisherService(RabbitMQSettings settings, ISerializer serializer) : RabbitMQService(settings, serializer), IUserPublisherService
{
    private readonly string _exchange = "settings-service";

    public async Task Create(CreateUser createUser)
    {
        await Publish(_exchange, "user.success.added", createUser);
        Console.WriteLine($"{createUser.UserId} publish");
    }

    public async Task Delete(CreateUser deleteUser)
    {
        await Publish(_exchange, "user.success.deleted", deleteUser);
        Console.WriteLine($"{deleteUser.UserId} deleted");
    }
}
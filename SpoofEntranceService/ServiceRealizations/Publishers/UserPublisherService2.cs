using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofEntranceService.Services.Publishers;

namespace SpoofEntranceService.ServiceRealizations.Publishers;

public class UserPublisherService2(
        RabbitMQSettings settings,
        ILoggerService loggerService,
        ISerializer serializer
    ) : PublisherService(
        settings,
        loggerService,
        serializer
        ), IUserPublisherService
{
    protected override string Exchange => "entrance-service";

    public async Task Create(CreateUser createUser)
    {
        await Publish("user.success.created", createUser);
        Console.WriteLine($"{createUser.UserId} publish");
    }

    public async Task Delete(CreateUser deleteUser)
    {
        await Publish("user.success.deleted", deleteUser);
        Console.WriteLine($"{deleteUser.UserId} deleted");
    }
}
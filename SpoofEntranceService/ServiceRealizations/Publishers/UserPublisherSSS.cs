using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofEntranceService.Services.Publishers;

namespace SpoofEntranceService.ServiceRealizations.Publishers;

public class UserPublisherSSS(
        RabbitMQSettings settings,
        ILoggerService loggerService,
        ISerializer serializer
    ) : PublisherService(
            settings,
            loggerService,
            serializer
        ), ISSSUserPublisherService
{
    protected override string Exchange => "settings-service";

    public async Task Create(CreateUser createUser)
    {
        await Publish("user.success.added", createUser);
        Console.WriteLine($"{createUser.UserId} publish");
    }

    public async Task Delete(CreateUser deleteUser)
    {
        await Publish("user.success.deleted", deleteUser);
        Console.WriteLine($"{deleteUser.UserId} deleted");
    }
}
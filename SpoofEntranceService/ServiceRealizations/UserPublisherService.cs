using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofEntranceService.Services;
using System.Text;

namespace SpoofEntranceService.ServiceRealizations;

public class UserPublisherService(RabbitMQSettings settings, ISerializer serializer) : RabbitMQService(settings, serializer), IUserPublisherService
{
    private readonly string _exchange = "settings-service";

    public async Task Create(CreateUser createUser)
    {
        Console.WriteLine($"{createUser.UserId} was created");
        byte[] body = Encoding.UTF8.GetBytes(_serializer.Serialize(createUser));
        await Publish(_exchange, "user.success.added", body);
    }
}
using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Services.MessageBrokers;
using System.Text;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers;

public class UserMessageService(RabbitMQSettings settings, ISerializer serializer) : RabbitMQService(settings, serializer), IUserMessageBrokerService
{
    private readonly string _exchange = "entrance-service";

    public async Task ConfirmCreate(CreateUser createUser)
    {
        byte[] body = Encoding.UTF8.GetBytes(_serializer.Serialize(createUser));
        await Publish(_exchange, "user.success.created", body);
    }

    public async Task ConfirmDelete (CreateUser createUser)
    {
        byte[] body = Encoding.UTF8.GetBytes(_serializer.Serialize(createUser));
        await Publish(_exchange, "user.success.deleted", body);
    }
    public async Task ErrorAdded(CreateUser createUser)
    {
        byte[] body = Encoding.UTF8.GetBytes(_serializer.Serialize(createUser));
        await Publish(_exchange, "user.error.added", body);
    }
}

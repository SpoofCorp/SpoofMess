using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Services.MessageBrokers;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers;

public class UserPublisherService(RabbitMQSettings settings, ISerializer serializer) : RabbitMQService(settings, serializer), IUserMessageBrokerService
{
    private readonly string _exchange = "entrance-service";

    public async Task ConfirmCreate(CreateUser createUser)
    {
        await Publish(_exchange, "user.success.created", createUser);
    }

    public async Task ConfirmDelete (CreateUser deletedUser)
    {
        await Publish(_exchange, "user.success.deleted", deletedUser);
    }
    public async Task ErrorAdded(CreateUser createUser)
    {
        await Publish(_exchange, "user.error.added", createUser);
    }
}

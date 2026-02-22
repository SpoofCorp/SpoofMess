using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Services.MessageBrokers;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers.Publishers;

public class UserPublisherService(
        RabbitMQSettings settings,
        ILoggerService loggerService,
        ISerializer serializer
    )
    : PublisherService(
            settings,
            loggerService,
            serializer
        ), IUserMessageBrokerService
{
    protected override string Exchange => "entrance-service";

    public async Task ConfirmCreate(CreateUser createUser) =>
        await Publish("user.success.created", createUser);

    public async Task ConfirmDelete(CreateUser deletedUser) =>
        await Publish("user.success.deleted", deletedUser);

    public async Task ErrorAdded(CreateUser createUser) =>
        await Publish("user.error.added", createUser);
}

using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Services.MessageBrokers;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers.Publishers;

public class ChatPublisherService(
    RabbitMQSettings settings,
    ISerializer serializer
    ) : RabbitMQService(
        settings,
        serializer
        ), IChatPublisherService
{
    protected override string Exchange => "message-service";

    public async Task Publish(CreateChat chat)
    {
        await Publish("chat.success.created", chat);
        Console.WriteLine($"Chat: {chat.Id} was created");
    }
}
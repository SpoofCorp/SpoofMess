using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.MessageBrokers;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers.Publishers;

public class ChatUserPublisherService(RabbitMQSettings settings, ISerializer serializer) : RabbitMQService(settings, serializer), IChatUserPublisherService
{
    protected override string Exchange => "message-service";

    public async Task Create(ChatUser chatUser, Rule[] rules)
    {
        CreateChatUser create = new(chatUser.Key2, chatUser.Key1, rules);
        await Publish("chatuser.create.success", create);
    }
}

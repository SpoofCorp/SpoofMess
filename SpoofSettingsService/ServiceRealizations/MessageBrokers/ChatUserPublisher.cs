using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.MessageBrokers;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers;

public class ChatUserPublisherService(RabbitMQSettings settings, ISerializer serializer) : RabbitMQService(settings, serializer), IChatUserPublisherService
{
    private readonly string _exchange = "message-service";
    public async Task Create(ChatUser chatUser, Rule[] rules)
    {
        CreateChatUser create = new(chatUser.Key2, chatUser.Key1, rules);
        await Publish(_exchange, "chatuser.create.success", create);
    }
}

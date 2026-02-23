using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.MessageBrokers;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers.Publishers;

public class ChatUserPublisherService(
        RabbitMQSettings settings,
        ILoggerService loggerService,
        ISerializer serializer
    ) : PublisherService(
            settings,
            loggerService,
            serializer
        ), IChatUserPublisherService
{
    protected override string Exchange => "message-service";


    public async Task Create(ChatUser chatUser, Rule[] rules)
    {
        CreateChatUser create = new(chatUser.Key2, chatUser.Key1, rules);
        await Create(create);
    }

    public async Task Update(ChatUser chatUser, Rule[] rules)
    {
        UpdateChatUser update = new(chatUser.Key2, chatUser.Key1, rules);
        await Update(update);
    }

    public async Task Delete(ChatUser chatUser)
    {
        DeleteChatUser delete = new(chatUser.Key2, chatUser.Key1);
        await Delete(delete);
    }

    public async Task Create(CreateChatUser createChatUser) =>
        await Publish("chatuser.create.success", createChatUser);

    public async Task Update(UpdateChatUser updateChatUser) =>
        await Publish("chatuser.update.success", updateChatUser);

    public async Task Delete(DeleteChatUser deleteChatUser) =>
        await Publish("chatuser.delete.success", deleteChatUser);
}
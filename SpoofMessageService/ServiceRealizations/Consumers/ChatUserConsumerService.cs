using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using CommunicationLibrary.Services;
using SpoofMessageService.Services;
using SpoofMessageService.Services.Consumers;

namespace SpoofMessageService.ServiceRealizations.Consumers;

public class ChatUserConsumerService(
    RabbitMQSettings settings,
    ISerializer serializer,
    ILoggerService loggerService,
    IInjectionService injectionService)
    : ConsumerService(
        settings,
        serializer,
        loggerService), IChatUserConsumerService

{
    private readonly IInjectionService _injectionService = injectionService;
    protected override string Exchange => "message-service";

    protected override string BaseQueueName => "message.chatuser";

    public override async Task Initialize()
    {
        await ConfirmAdded();
        await ConfirmUpdated();
    }

    public async Task ConfirmAdded() =>
        await ConsumeFromQueueAsync<CreateChatUser>("create.success", "chatuser.create.success", async (chatUser) =>
        {
            await _injectionService.Invoke<IChatUserService, Task>(async (chatUserService) =>
            {
                await chatUserService.Add(chatUser);
            });
        });

    public async Task ConfirmDeleted() =>
        await ConsumeFromQueueAsync<DeleteChatUser>("delete.success", "chatuser.deleted.success", async (chatUser) =>
        {
            await _injectionService.Invoke<IChatUserService, Task>(async (chatUserService) =>
            {
                await chatUserService.Delete(chatUser.ChatId, chatUser.UserId);
            });
        });
    public async Task ConfirmUpdated() =>
        await ConsumeFromQueueAsync<UpdateChatUser>("update.success", "chatuser.update.success", async (chatUser) =>
        {
            await _injectionService.Invoke<IChatUserService, Task>(async (chatUserService) =>
            {
                await chatUserService.Update(chatUser);
            });
        });
}

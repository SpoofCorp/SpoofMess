using AdditionalHelpers.Services;
using CommonObjects.Results;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using CommunicationLibrary.Services;
using SpoofMessageService.Services;

namespace SpoofMessageService.ServiceRealizations.Consumers;

public class ChatConsumerService(
    RabbitMQSettings settings, 
    IInjectionService injectionService, 
    ISerializer serializer, 
    ILoggerService loggerService
    ) : ConsumerService(
        settings, 
        serializer, 
        loggerService
        )
{
    protected readonly IInjectionService _injectionService = injectionService;

    protected override string BaseQueueName => "message.chat";

    protected override string Exchange => "message-service";


    public override async Task Initialize()
    {
        await SuccessCreated();
    }


    private async Task SuccessCreated()
    {
        await ConsumeFromQueueAsync<CreateChat>("success.created", "chat.success.created", async (createChat) =>
        {
            await _injectionService.Invoke<IChatService, Task<Result>>(async (chatService) =>
            {
                return await chatService.Create(createChat);
            });
        });
    }
}

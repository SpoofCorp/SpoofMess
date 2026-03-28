using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using CommunicationLibrary.Services;
using SpoofMessageService.Services;

namespace SpoofMessageService.ServiceRealizations.Consumers;

public class UserSESConsumerService(
    RabbitMQSettings settings,
    ISerializer serializer,
    ILoggerService loggerService,
    IInjectionService injectionService
    ) : ConsumerService(
        settings,
        serializer,
        loggerService
        )
{
    protected readonly IInjectionService _injectionService = injectionService;
    protected override string Exchange => "entrance-service";
    protected override string BaseQueueName => "message.user";

    public override async Task Initialize()
    {
        await ConfirmCreated();
    }

    public async Task ConfirmCreated()
    {
        await ConsumeFromQueueAsync<CreateUser>(
            "success.created", 
            "user.success.created",
            async (createUser) =>
            {
                await _injectionService.Invoke<IUserService, Task>(
                    async (userEntryService) => await userEntryService.Create(createUser));
                _loggerService.Info($"{createUser.UserId} was created");
            });
    }
}

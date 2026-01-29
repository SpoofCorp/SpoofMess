using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using CommunicationLibrary.Services;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.MessageBrokers;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers;

public class UserConsumerService(RabbitMQSettings settings, ISerializer serializer, ILoggerService loggerService, IInjectionService injectionService) : ConsumerService(settings, serializer, loggerService), IUserConsumerService
{
    private readonly string _exchange = "settings-service";
    protected readonly IInjectionService _injectionService = injectionService;

    protected async Task ConfirmAdded() =>
        await ConsumeFromQueueAsync<CreateUser>(
            _exchange,
            $"settings.user.success.added",
            $"user.success.added",
            async (createUser)  =>
                {
                    _loggerService.Info($"{createUser.UserId} was created");

                    await _injectionService.Invoke<IUserService, Task>(
                        async (userService) =>
                            await userService.Create(createUser.UserId)
                        );
                }
            );

    public override async Task Initialize()
    {
        await ConfirmAdded();
    }
}
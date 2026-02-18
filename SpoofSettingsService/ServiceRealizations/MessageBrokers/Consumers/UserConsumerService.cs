using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using CommunicationLibrary.Services;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.MessageBrokers;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers.Consumers;

public class UserConsumerService(RabbitMQSettings settings, ISerializer serializer, ILoggerService loggerService, IInjectionService injectionService) : ConsumerService(settings, serializer, loggerService), IUserConsumerService
{
    protected override string BaseQueueName => "settings.user";
    protected override string Exchange => "settings-service";

    protected readonly IInjectionService _injectionService = injectionService;

    protected async Task ConfirmAdded() =>
        await ConsumeFromQueueAsync<CreateUser>(
            $"success.added",
            $"user.success.added",
            async (createUser)  =>
                {
                    _loggerService.Info($"{createUser.UserId} was created");

                    await _injectionService.Invoke<IUserService, Task>(
                        async (userService) =>
                            await userService.Create(createUser)
                        );
                });

    protected async Task ConfirmDeleted() =>
        await ConsumeFromQueueAsync<CreateUser>(
            $"success.deleted",
            $"user.success.deleted",
            async (createUser) =>
            {
                _loggerService.Info($"{createUser.UserId} was deleted");

                await _injectionService.Invoke<IUserService, Task>(
                        async (userService) =>
                            await userService.Delete(createUser.UserId)
                        );
            });

    public override async Task Initialize()
    {
        await ConfirmAdded();
        await ConfirmDeleted();
    }
}
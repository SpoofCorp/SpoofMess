using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using CommunicationLibrary.Services;
using SpoofEntranceService.Services;

namespace SpoofEntranceService.ServiceRealizations;

public class UserConsumerService(RabbitMQSettings settings, ISerializer serializer, ILoggerService loggerService, IInjectionService injectionService) : ConsumerService(settings, serializer, loggerService), IUserConsumerService
{
    protected readonly IInjectionService _injectionService = injectionService;
    private readonly string _exchange = "entrance-service";

    public override async Task Initialize()
    {
        await ConfirmAdded();
        await ErrorAdded();
        await ConfirmDelete();
    }

    private async Task ConfirmAdded()
    {
        await ConsumeFromQueueAsync<CreateUser>(_exchange, "entrance.user.success.created", "user.success.created", async (createUser) =>
        {
            _loggerService.Info($"{createUser.UserId} was created");
            await _injectionService.Invoke<IUserEntryService, Task>(async (userEntryService) => await userEntryService.Confirm(createUser.UserId));
        });
    }

    private async Task ErrorAdded()
    {
        await ConsumeFromQueueAsync<CreateUser>(_exchange, "entrance.user.error.created", "user.error.created", async (createUser) =>
        {
            await _injectionService.Invoke<IUserEntryService, Task>(async (userEntryService) => await userEntryService.Error(createUser.UserId));
        });
    }

    private async Task ConfirmDelete()
    {
        await ConsumeFromQueueAsync<CreateUser>(_exchange, "entrance.user.success.deleted", "user.success.deleted", async (createUser) =>
        {
            await _injectionService.Invoke<IUserEntryService, Task>(async (userEntryService) => await userEntryService.Delete(createUser.UserId));
        });
    }
}
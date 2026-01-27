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
    private readonly string _exchange = "settings-service";

    public override async Task Initialize()
    {
        await ConfirmAdded();
        await ErrorAdded();
        await ConfirmDelete();
    }

    private async Task ConfirmAdded()
    {
        await ConsumeFromQueueAsync<CreateUser>(_exchange, "user.success", "user.success.created", async (createUser) =>
        {
            IUserEntryService userEntryService = _injectionService.GetService<IUserEntryService>();
            _loggerService.Info($"{createUser.UserId} was created");
            await userEntryService.Confirm(createUser.UserId);
        });
    }

    private async Task ErrorAdded()
    {
        await ConsumeFromQueueAsync<CreateUser>(_exchange, "user.error", "user.error.created", async (createUser) =>
        {
            IUserEntryService userEntryService = _injectionService.GetService<IUserEntryService>();
            await userEntryService.Error(createUser.UserId);
        });
    }

    private async Task ConfirmDelete()
    {
        await ConsumeFromQueueAsync<CreateUser>(_exchange, "user.success", "user.success.deleted", async (createUser) =>
        {
            IUserEntryService userEntryService = _injectionService.GetService<IUserEntryService>();
            await userEntryService.Delete(createUser.UserId);
        });
    }
}
using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofEntranceService.Services;

namespace SpoofEntranceService.ServiceRealizations;

public class UserConsumerService(RabbitMQSettings settings, ISerializer serializer, IUserEntryService userEntryService) : RabbitMQService(settings, serializer), IUserConsumerService
{
    private readonly IUserEntryService _userEntryService = userEntryService;
    private readonly string _exchange = "settings-service";

    public async Task Initialize()
    {
        await ConfirmAdded();
        await ErrorAdded();
        await ConfirmDelete();
    }

    private async Task ConfirmAdded()
    {
        await ConsumeFromQueueAsync<CreateUser>(_exchange, "user.success", "user.success.created", async (createUser) =>
        {
            await _userEntryService.Confirm(createUser.UserId);
        });
    }

    private async Task ErrorAdded()
    {
        await ConsumeFromQueueAsync<CreateUser>(_exchange, "user.error", "user.error.created", async (createUser) =>
        {
            await _userEntryService.Error(createUser.UserId);
        });
    }

    private async Task ConfirmDelete()
    {
        await ConsumeFromQueueAsync<CreateUser>(_exchange, "user.success", "user.success.deleted", async (createUser) =>
        {
            await _userEntryService.Delete(createUser.UserId);
        });
    }
}
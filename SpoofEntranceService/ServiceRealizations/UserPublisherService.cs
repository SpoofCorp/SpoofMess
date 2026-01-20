using AdditionalHelpers.Services;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofEntranceService.Services;
using System.Text;

namespace SpoofEntranceService.ServiceRealizations;

public class UserPublisherService : RabbitMQService, IUserPublisherService
{
    private readonly IUserEntryService _userEntryService;
    private readonly string _exchange = "settings-service";
    public UserPublisherService(string hostName, int port, ISerializer serializer, IUserEntryService userEntryService) : base(hostName, port, serializer)
    {
        _userEntryService = userEntryService;
        _ = ConfirmAdded();
        _ = ErrorAdded();
        _ = ConfirmDelete();
    }

    public async Task Create(CreateUser createUser)
    {
        byte[] body = Encoding.UTF8.GetBytes(_serializer.Serialize(createUser));
        await Publish(_exchange, "user.created", body);
    }

    private async Task ConfirmAdded()
    {
        await ConsumeFromQueueAsync<CreateUser>(_exchange, "user.success", "user.success.created", async(createUser) =>
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

using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.MessageBrokers;
using System.Text;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers;

public class UserMessageService : RabbitMQService, IUserMessageBrokerService
{
    private readonly string _exchange = "settings-service";
    private readonly IUserService _userService;
    public UserMessageService(RabbitMQSettings settings, ISerializer serializer, IUserService userService) : base(settings, serializer)
    {
        _userService = userService;
        
    }

    public async Task ConfirmCreate(CreateUser createUser)
    {
        byte[] body = Encoding.UTF8.GetBytes(_serializer.Serialize(createUser));
        await Publish(_exchange, "user.success.created", body);
    }

    public async Task ConfirmDelete (CreateUser createUser)
    {
        byte[] body = Encoding.UTF8.GetBytes(_serializer.Serialize(createUser));
        await Publish(_exchange, "user.success.created", body);
    }
    public async Task ErrorAdded(CreateUser createUser)
    {
        byte[] body = Encoding.UTF8.GetBytes(_serializer.Serialize(createUser));
        await Publish(_exchange, "user.success.created", body);
    }

    private async Task Create()
    {
        await ConsumeFromQueueAsync<CreateUser>(_exchange, "user.error", "user.error.created", async (createUser) =>
        {
            await _userService.Create(createUser.UserId);
        });
    }

}

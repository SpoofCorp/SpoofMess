using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.MessageBrokers;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers;

public class UserConsumerService(RabbitMQSettings settings, ISerializer serializer, IUserService userService) : FileMessageConsumerService<User>(settings, serializer), IUserConsumerService
{
    private readonly IUserService _userService = userService;
    private readonly string _exchange = "settings-service";

    protected override string FileNomination => throw new NotImplementedException();

    public override async Task Initialize()
    {
        await ConsumeFromQueueAsync<CreateUser>(_exchange, "user.error", "user.error.created", async (createUser) =>
        {
            await _userService.Create(createUser.UserId);
        });
    }
}
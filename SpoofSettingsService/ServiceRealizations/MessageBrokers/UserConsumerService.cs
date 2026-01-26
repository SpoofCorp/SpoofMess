using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.MessageBrokers;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers;

public class UserConsumerService(RabbitMQSettings settings, ISerializer serializer, ILoggerService loggerService) : ConsumerService(settings, serializer, loggerService), IUserConsumerService
{
    private readonly string _exchange = "settings-service";
    //private readonly IUserService _userService;

    protected async Task ConfirmAdded() =>
        await ConsumeFromQueueAsync<CreateUser>(_exchange, $"user.success", $"user.success.added", async (createUser)  => {
            _loggerService.Info($"{createUser.UserId} was created");
            //await _userService.Create(createUser.UserId);
        });

    public override async Task Initialize()
    {
        await ConfirmAdded();
    }
}
using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using CommunicationLibrary.Services;
using SpoofMessageService.Services;

namespace SpoofMessageService.ServiceRealizations.Consumers;

public class UserSSSConsumerService(
    RabbitMQSettings settings,
    ISerializer serializer,
    ILoggerService loggerService,
    IServiceScopeFactory serviceFactory
    ) : ConsumerService(
        settings,
        serializer,
        loggerService
        )
{
    private readonly IServiceScopeFactory _serviceFactory = serviceFactory;
    protected IUserService _userService = null!;
    protected override string Exchange => "settings-service";
    protected override string BaseQueueName => "message.user";

    public override async Task Initialize()
    {
        IServiceScope scope = _serviceFactory.CreateScope();
        _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        await ConfirmUpdated();
    }

    public async Task ConfirmUpdated()
    {
        await ConsumeFromQueueAsync<UpdateUser>(
            "success.updated",
            "user.success.updated",
            async (updateUser) =>
            {
                await _userService.Update(updateUser);
                _loggerService.Info($"{updateUser.UserId} was updated");
            }
        );
    }
}

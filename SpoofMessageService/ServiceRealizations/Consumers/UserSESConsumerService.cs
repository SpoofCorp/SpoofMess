using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using CommunicationLibrary.Services;
using SpoofMessageService.Services;

namespace SpoofMessageService.ServiceRealizations.Consumers;

public class UserSESConsumerService(
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
    protected override string Exchange => "entrance-service";
    protected override string BaseQueueName => "message.user";

    public override async Task Initialize()
    {
        IServiceScope scope = _serviceFactory.CreateScope();
        _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        await ConfirmCreated();
    }

    public async Task ConfirmCreated()
    {
        await ConsumeFromQueueAsync<CreateUser>(
            "success.created", 
            "user.success.created",
            async (createUser) =>
            {
                await _userService.Create(createUser);
                _loggerService.Info($"{createUser.UserId} was created");
            }
        );
    }
}
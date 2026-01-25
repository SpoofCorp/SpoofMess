using AdditionalHelpers.Services;
using CommunicationLibrary;
using CommunicationLibrary.Communication;
using CommunicationLibrary.ServiceRealizations;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.MessageBrokers;

namespace SpoofSettingsService.ServiceRealizations.MessageBrokers;

public class UserConsumerService(RabbitMQSettings settings, ISerializer serializer, IUserService userService) : FileMessageConsumerService<CreateUser>(settings, serializer), IUserConsumerService
{
    private readonly IUserService _userService = userService;
    private readonly string _exchange = "settings-service";

    protected override string FileNomination => throw new NotImplementedException();

    protected override Func<CreateUser, Task> ConfirmDeletedFunc => throw new NotImplementedException();

    protected override Func<CreateUser, Task> ConfirmAddedFunc => async (createUser) => await _userService.Create(createUser.UserId);

    protected override Func<CreateUser, Task> ErrorDeletedFunc => throw new NotImplementedException();

    protected override Func<CreateUser, Task> ErrorAddedFunc => throw new NotImplementedException();
}
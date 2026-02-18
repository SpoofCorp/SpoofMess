using CommunicationLibrary.Communication;
using SpoofEntranceService.Services.Publishers;

namespace SpoofEntranceService.ServiceRealizations.Publishers;

public class UserPublisherService(ISSSUserPublisherService userPublisherSSSService, ISMSUserPublisherService userPublisherSMSService) : IUserPublisherService
{
    private readonly ISSSUserPublisherService _userPublisherSSSService = userPublisherSSSService;
    private readonly ISMSUserPublisherService _userPublisherSMSService = userPublisherSMSService;

    public async Task Create(CreateUser createUser)
    {
        await _userPublisherSSSService.Create(createUser);
        await _userPublisherSMSService.Create(createUser);
        Console.WriteLine($"{createUser.UserId} publish");
    }

    public async Task Delete(CreateUser deleteUser)
    {
        await _userPublisherSSSService.Delete(deleteUser);
        await _userPublisherSMSService.Delete(deleteUser);
        Console.WriteLine($"{deleteUser.UserId} deleted");
    }
}
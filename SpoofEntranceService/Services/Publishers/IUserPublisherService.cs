using CommunicationLibrary.Communication;

namespace SpoofEntranceService.Services.Publishers;

public interface IUserPublisherService
{
    public Task Create(CreateUser createUser);

    [Obsolete("It's not SES responsibility! SES only creates point of user, when SSS only deletes and edits point of users")]
    public Task Delete(CreateUser deleteUser); 
}

using CommonObjects.DTO;
using CommonObjects.Requests.Changes;
using CommonObjects.Results;
using CommunicationLibrary.Communication;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services;

public interface IUserService
{
    public Task<Result> ChangeSettings(ChangeUserSettingsRequest request, Guid userId);

    public Task<Result> Delete(Guid userId);
    public Task<Result<UserDTO>> GetInfo(Guid id);
    public Task<Result> Create(CreateUser createUser);
    public Task<Result<User>> Get(Guid id);

}

using DataSaveHelpers.Services.Repositories;
using SpoofMessageService.Models;

namespace SpoofMessageService.Services.Repositories;

public interface IUserRepository : IIdentifiedRepository<User, Guid>
{
    public Task<bool> ExecuteUpdate(Guid userId, bool isConnected);
}

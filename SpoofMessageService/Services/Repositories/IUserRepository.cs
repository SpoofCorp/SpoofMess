using DataSaveHelpers.Services.Repositories;
using SpoofMessageService.Models;

namespace SpoofMessageService.Services.Repositories;

public interface IUserRepository : IIdentifiedRepository<User, Guid>
{
}

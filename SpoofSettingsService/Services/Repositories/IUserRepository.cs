using DataHelpers.Services.Repositories;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Repositories;

public interface IUserRepository : ISoftDeletableIdentifiedRepository<User, Guid>
{
}

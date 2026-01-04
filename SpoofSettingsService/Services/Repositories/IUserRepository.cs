using DataHelpers.Services;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Interfaces;

public interface IUserRepository : IBaseRepository<User, Guid>
{
}

using DataSaveHelpers.Services.Repositories;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Repositories;

public interface IRoleRepository : ISoftDeletableIdentifiedRepository<Role, int>
{
}

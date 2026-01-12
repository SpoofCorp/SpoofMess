using SpoofSettingsService.Models;
using SpoofSettingsService.Services;

namespace SpoofSettingsService.ServiceRealizations;

public class RoleService : IRoleService
{
    public Task<Role?> GetRoleById(long roleId)
    {
        throw new NotImplementedException();
    }
}

using SpoofSettingsService.Models;
using SpoofSettingsService.Services;

namespace SpoofSettingsService.ServiceRealizations;

public class RoleTypeService : IRoleTypeService
{
    public Task<RoleType?> GetRoleById(long roleId)
    {
        throw new NotImplementedException();
    }
}

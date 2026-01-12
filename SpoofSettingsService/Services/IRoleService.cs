using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services;

public interface IRoleService
{
    public Task<Role?> GetRoleById(long roleId);
}

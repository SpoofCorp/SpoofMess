using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services;

public interface IChatRoleService
{
    public Task<Result<ChatRole>> Get(long id);
}

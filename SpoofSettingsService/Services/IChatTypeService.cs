using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services;

[Obsolete("Not check user permissions")]
public interface IChatTypeService
{
    public Task<Result<ChatType>> Get(int Id);
}

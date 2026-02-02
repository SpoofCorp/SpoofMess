using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services;

public interface IChatTypeService
{
    public Task<Result<ChatType>> Get(int Id);
}

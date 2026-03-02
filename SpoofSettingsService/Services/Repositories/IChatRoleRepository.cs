using DataSaveHelpers.Services.Repositories;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Repositories;

public interface IChatRoleRepository : ISoftDeletableIdentifiedRepository<ChatRole, long>
{
}

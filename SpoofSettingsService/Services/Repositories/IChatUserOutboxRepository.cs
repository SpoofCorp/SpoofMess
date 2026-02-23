using DataSaveHelpers.Services.Repositories;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Repositories;

public interface IChatUserOutboxRepository : IIdentifiedRepository<ChatUserOutbox, Guid>
{
    public Task<List<ChatUserOutbox>> GetNotSynced(DateTime notBefore);
}

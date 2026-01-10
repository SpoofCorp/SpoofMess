using DataHelpers.Services.Repositories;
using SpoofEntranceService.Models;

namespace SpoofEntranceService.Services.Repositories;

public interface ISessionRepository : ISoftDeletableIdentifiedRepository<SessionInfo, Guid>
{
    public ValueTask<List<SessionInfo>?> GetSessionsByUserId(Guid userId);

    public Task SoftDelete(SessionInfo entity);

    public Task SoftDeleteSessionsByUserId(Guid userId, bool withCurrent = false, Guid? currentSessionId = null);
}

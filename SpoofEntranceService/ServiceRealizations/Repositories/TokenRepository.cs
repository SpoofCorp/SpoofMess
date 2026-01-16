using DataSaveHelpers.Services;
using SpoofEntranceService.Models;
using Microsoft.EntityFrameworkCore;
using SpoofEntranceService.Services.Repositories;
using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;

namespace SpoofEntranceService.ServiceRealizations.Repositories;

public class TokenRepository(ICacheService cache, SpoofEntranceServiceDbContext context, IProcessQueueTasksService tasksService) : CachedSoftDeletableIdentifiedRepository<Token, string>(cache, context, tasksService), ITokenRepository
{
    public async ValueTask Add(Token token) =>
        await AddAsync(token);

    public async ValueTask<Token?> GetByRefreshHash(string refreshHash) =>
        await GetAsync(GetKey(refreshHash),
            async () => await _set
                .Include(x => x.SessionInfo)
                .ThenInclude(x => x.UserEntry)
                .FirstOrDefaultAsync(x => refreshHash == x.Id));

    public async Task Replace(Token replaced, Token replacing)
    {
        replaced.IsDeleted = true;
        _context.Entry(replaced).State = EntityState.Modified;
        await _set.AddAsync(replacing);
        await _context.SaveChangesAsync();
    }
}

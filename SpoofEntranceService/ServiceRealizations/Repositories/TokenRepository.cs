using AdditionalHelpers.Services;
using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofEntranceService.Models;
using SpoofEntranceService.Services.Repositories;

namespace SpoofEntranceService.ServiceRealizations.Repositories;

public class TokenRepository(ICacheService cache, ISerializer serializer, SpoofEntranceServiceContext context, IProcessQueueTasksService tasksService) : CachedSoftDeletableIdentifiedRepository<Token, string>(cache, context, tasksService), ITokenRepository
{
    private readonly ISerializer _serializer = serializer;
    public async Task SaveTokenAndSession(Token token)
    {
        await context.SessionInfos.AddAsync(token.SessionInfo);
        await context.Tokens.AddAsync(token);
        await context.SaveChangesAsync();
        _processQueueTasks.AddTask(async () => await _cache.MultiSave(
                [
                new(GetEntityKey<SessionInfo, Guid>(token.SessionInfo), _serializer.Serialize(token.SessionInfo)),
                new(GetEntityKey<Token, string>(token), _serializer.Serialize(token)),
            ]
            ));
    }

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

using AdditionalHelpers.Services;
using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofEntranceService.Models;
using SpoofEntranceService.Services.Repositories;

namespace SpoofEntranceService.ServiceRealizations.Repositories;

public class TokenRepository(
        ICacheService cache,
        ISerializer serializer,
        IDbContextFactory<SpoofEntranceServiceContext> factory,
        IProcessQueueTasksService tasksService
    ) : CachedSoftDeletableIdentifiedFactoryRepository<Token, string, SpoofEntranceServiceContext>(
        cache, 
        factory, 
        tasksService
    ), ITokenRepository
{
    private readonly ISerializer _serializer = serializer;
    public async Task SaveTokenAndSession(Token token)
    {
        await using SpoofEntranceServiceContext context = await _factory.CreateDbContextAsync();
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

    public async ValueTask<Token?> GetByRefreshHash(string refreshHash)
    {
        await using SpoofEntranceServiceContext context = await _factory.CreateDbContextAsync();
        return await GetAsync(GetKey(refreshHash),
            async () => await context.Tokens
                .Include(x => x.SessionInfo)
                .ThenInclude(x => x.UserEntry)
                .FirstOrDefaultAsync(x => refreshHash == x.Id)
            );
    }

    public async Task Replace(Token replaced, Token replacing)
    {
        await using SpoofEntranceServiceContext context = await _factory.CreateDbContextAsync();
        replaced.IsDeleted = true;
        context.Entry(replaced).State = EntityState.Modified;
        await context.Tokens.AddAsync(replacing);
        await context.SaveChangesAsync();
    }
}

using AdditionalHelpers.Services;
using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofEntranceService.Models;
using SpoofEntranceService.Services.Repositories;

namespace SpoofEntranceService.ServiceRealizations.Repositories;

public class UserRepository(
    ICacheService cache,
    ISerializer serializer,
    IDbContextFactory<SpoofEntranceServiceContext> factory,
    IProcessQueueTasksService tasksService
    ) : CachedSoftDeletableIdentifiedFactoryRepository<UserEntry, Guid, SpoofEntranceServiceContext>(
        cache,
        factory, 
        tasksService
    ), IUserEntryRepository
{
    private readonly ISerializer _serializer = serializer;
    public async Task Create(UserEntry entry, SessionInfo session, Token token)
    {
        await using SpoofEntranceServiceContext context = await _factory.CreateDbContextAsync();
        await context.UserEntries.AddAsync(entry);
        await context.SessionInfos.AddAsync(session);
        await context.Tokens.AddAsync(token);
        await context.SaveChangesAsync();
        _processQueueTasks.AddTask(async () => await _cache.MultiSave(
                [
                new(GetKey(entry), _serializer.Serialize(entry)),
                new(GetKey(entry.Id), _serializer.Serialize(entry)),
                new(GetEntityKey<SessionInfo, Guid>(session), _serializer.Serialize(session)),
                new(GetEntityKey<Token, string>(token), _serializer.Serialize(token)),
            ]
            ));
    }

    public async Task Change(UserEntry? oldUser)
    {
        if (oldUser is not null)
        {
            await using SpoofEntranceServiceContext context = await _factory.CreateDbContextAsync();
            oldUser.IsDeleted = true;
            oldUser.UniqueName = Guid.CreateVersion7().ToString();
            context.Entry(oldUser).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }

    protected override string GetKey(UserEntry entity) =>
        $"{typeof(UserEntry).Name}:{entity.UniqueName}".ToLower();

    public async Task<UserEntry?> GetByLogin(string login) =>
        await GetAsync(GetKeyByLogin(login), async () =>
        {
            await using SpoofEntranceServiceContext context = await _factory.CreateDbContextAsync();
            return await context.UserEntries.FirstOrDefaultAsync(x => x.UniqueName == login);
        });

    private static string GetKeyByLogin(string login) =>
        $"{typeof(UserEntry).Name}:{login}".ToLower();
}

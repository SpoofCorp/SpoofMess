using DataHelpers.ServiceRealizations.Repositories.WithCache;
using DataHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofEntranceService.Models;
using SpoofEntranceService.Services.Repositories;

namespace SpoofEntranceService.ServiceRealizations.Repositories;

public class UserRepository(ICacheService cache, SpoofEntranceServiceDbContext context, IProcessQueueTasksService tasksService) : CachedSoftDeletableIdentifiedRepository<UserEntry, Guid>(cache, context, tasksService), IUserEntryRepository
{
    public async Task Change(UserEntry newUser, UserEntry? oldUser)
    {
        if (oldUser is null)
            await AddAsync(newUser);
        else
        {
            oldUser.IsDeleted = true;
            oldUser.UniqueName = Guid.CreateVersion7().ToString();
            _context.Entry(oldUser).State = EntityState.Modified;
            await _set.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }
    }

    public async ValueTask<UserEntry?> GetByLogin(string login) =>
        await GetAsync(GetKeyByLogin(login), async () => await context.UserEntries.FirstOrDefaultAsync(x => x.UniqueName == login));

    private static string GetKeyByLogin(string login) =>
        $"{typeof(UserEntry).Name}:login:{login}".ToLower();
}

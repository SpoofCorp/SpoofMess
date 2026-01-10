using DataHelpers.Services.Repositories;
using SpoofEntranceService.Models;

namespace SpoofEntranceService.Services.Repositories;

public interface IUserEntryRepository : ISoftDeletableIdentifiedRepository<UserEntry, Guid>
{
    public Task Change(UserEntry newUser, UserEntry? oldUser);
    public ValueTask<UserEntry?> GetByLogin(string login);
}

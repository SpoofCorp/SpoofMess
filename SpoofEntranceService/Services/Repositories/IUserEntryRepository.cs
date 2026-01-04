using DataHelpers.Services;
using SpoofEntranceService.Models;

namespace SpoofEntranceService.Services.Repositories;

public interface IUserEntryRepository : IBaseRepository<UserEntry, Guid>
{
    public Task Change(UserEntry newUser, UserEntry? oldUser);
    public ValueTask<UserEntry?> GetByLogin(string login);
}

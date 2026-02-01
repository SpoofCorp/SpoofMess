using DataSaveHelpers.Services.Repositories;
using SpoofEntranceService.Models;

namespace SpoofEntranceService.Services.Repositories;

public interface IUserEntryRepository : ISoftDeletableIdentifiedRepository<UserEntry, Guid>
{
    public Task Change(UserEntry? oldUser);
    public Task<UserEntry?> GetByLogin(string login);
    public Task Create(UserEntry entry, SessionInfo session, Token token);
}

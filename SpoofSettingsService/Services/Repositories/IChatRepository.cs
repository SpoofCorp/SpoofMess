using DataHelpers.Services;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Interfaces;

public interface IChatRepository : IBaseRepository<Chat, Guid>
{
    public Task<Chat?> GetByUniqueName(string name);
    public Task Change(Chat newChat, Chat? oldChat);
}

using DataSaveHelpers.Services.Repositories;
using SpoofMessageService.Models;

namespace SpoofMessageService.Services.Repositories;

public interface IChatUserRepository : IDoubleIdentifiedRepository<ChatUser, Guid, Guid>
{
}

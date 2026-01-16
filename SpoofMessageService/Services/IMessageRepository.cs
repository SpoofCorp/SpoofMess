using DataSaveHelpers.Services.Repositories;
using SpoofMessageService.Models;

namespace SpoofMessageService.Services;

public interface IMessageRepository : ISoftDeletableIdentifiedRepository<Message, Guid>
{
}

using AdditionalHelpers.Services;
using CommonObjects.Results;
using SpoofMessageService.Models;
using SpoofMessageService.Models.Enums;

namespace SpoofMessageService.Services;

public interface IChatUserService
{
    public Task<Result<ChatUser>> Get(Guid chatId, Guid userId);

    public Task<Result<ChatUser>> GetAndCheckPermission(Guid chatId, Guid userId, Rules rule);
}

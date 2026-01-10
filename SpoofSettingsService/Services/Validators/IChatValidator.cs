using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Validators;

public interface IChatValidator : ISoftDeletableValidator<Chat>
{
    public Result ValidateChatType(ChatType? chatType);
    public Result ValidateHasChatUniqueName(Chat? chat);
    public Result ValidateChatAndOwner(Chat? chat, Guid userId);
}

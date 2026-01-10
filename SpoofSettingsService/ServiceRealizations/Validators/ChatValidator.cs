using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class ChatValidator : SoftDeletableValidator<Chat>, IChatValidator
{
    public Result ValidateChatType(ChatType? chatType)
    {
        if (chatType is null)
            return Result.NotFoundResult("Chat type is not exist");

        if (chatType.IsDeleted)
            return Result.BadRequest("ChatType is deleted");

        return Result.OkResult();
    }

    public Result ValidateHasChatUniqueName(Chat? chat)
    {
        if (chat is not null && !chat.IsDeleted)
            return Result.BadRequest("Unique name of chat has exist");

        return Result.OkResult();
    }

    public Result ValidateChatAndOwner(Chat? chat, Guid userId)
    {
        Result result = IsAvailable(chat);
        if (!result.Success)
            return result;
        if (chat!.OwnerId == userId)
            return Result.OkResult();
        else
            return Result.Forbidden("You is not owner");
    }
}
using CommonObjects.Results;
using DataSaveHelpers.ServiceRealizations;
using SpoofMessageService.Models;
using SpoofMessageService.Services.Validators;

namespace SpoofMessageService.ServiceRealizations.Validators;

public class MessageValidator : SoftDeletableValidator<Message>, IMessageValidator
{
    public Result IsAvailableAndOwner(Message? message, Guid? userId)
    {
        Result result = IsAvailable(message);
        if (!result.Success)
            return result;
        if (message!.UserId == userId)
            return Result.OkResult();

        return Result.Forbidden("It's not your message");
    }
}

using CommonObjects.Results;
using DataSaveHelpers.Services;
using SpoofMessageService.Models;

namespace SpoofMessageService.Services.Validators;

public interface IMessageValidator : ISoftDeletableValidator<Message>
{
    public Result IsAvailableAndOwner(Message? message, Guid? userId);
}

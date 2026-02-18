using CommonObjects.Results;
using DataSaveHelpers.Services;
using SpoofMessageService.Models;
using SpoofMessageService.Models.Enums;

namespace SpoofMessageService.Services.Validators;

public interface IChatUserValidator : ISoftDeletableValidator<ChatUser>
{
    public Result IsAvailableAndHasPermission(ChatUser? chatUser, Rules rule);
    public Result HasPermission(ChatUser chatUser, Rules rule);
}

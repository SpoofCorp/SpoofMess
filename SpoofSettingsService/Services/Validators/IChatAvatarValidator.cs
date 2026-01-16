using CommonObjects.Results;
using DataSaveHelpers.Services;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Validators;

public interface IChatAvatarValidator : ISoftDeletableValidator<ChatAvatar>
{
    public Result FileIsActive(ChatAvatar? chatAvatar);
}

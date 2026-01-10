using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Validators;

public interface IUserAvatarValidator : ISoftDeletableValidator<UserAvatar>
{
    public Result FileIsActive(UserAvatar? userAvatar);
}
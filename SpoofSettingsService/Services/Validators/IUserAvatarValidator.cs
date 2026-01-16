using CommonObjects.Results;
using SpoofSettingsService.Models;
using DataSaveHelpers.Services;

namespace SpoofSettingsService.Services.Validators;

public interface IUserAvatarValidator : ISoftDeletableValidator<UserAvatar>
{
    public Result FileIsActive(UserAvatar? userAvatar);
}
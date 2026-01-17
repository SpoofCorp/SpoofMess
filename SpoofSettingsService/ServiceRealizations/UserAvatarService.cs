using AdditionalHelpers.Services;
using CommonObjects.Requests.Avatars;
using CommonObjects.Responses;
using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;
using SpoofSettingsService.Setters;

namespace SpoofSettingsService.ServiceRealizations;

public class UserAvatarService(ILoggerService loggerService, IUserAvatarRepository userAvatarRepository, IUserAvatarValidator userAvatarValidator) : IUserAvatarService
{
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IUserAvatarRepository _userAvatarRepository = userAvatarRepository;
    private readonly IUserAvatarValidator _userAvatarValidator = userAvatarValidator;

    public async Task<Result<AvatarResponse>> GetAvatar(GetUserAvatarRequest request)
    {
        try
        {
            UserAvatar? avatar = await _userAvatarRepository.GetActualUserAvatarById(request.UserId);
            Result result = _userAvatarValidator.IsAvailable(avatar);
            if (!result.Success)
                return Result<AvatarResponse>.From(result);

            return Result<AvatarResponse>.SuccessResult(new() { FileId = avatar!.FileId, FileMetadata = avatar.File!.Set() });
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result<AvatarResponse>.ErrorResult("DataBase error");
        }
    }

    public async Task<Result<List<AvatarResponse>>> GetAvatars(GetUserAvatarRequest request)
    {
        try
        {
            List<UserAvatar>? avatars = await _userAvatarRepository.GetUserAvatarsById(request.UserId);
            Result result = _userAvatarValidator.IsAvailableCollection(avatars);
            if (!result.Success)
                return Result<List<AvatarResponse>>.From(result);

            UserAvatar avatar = null!;
            Result<List<AvatarResponse>> response = Result<List<AvatarResponse>>.OkResult([]);

            for (int i = 0; i < avatars!.Count; i++)
            {
                avatar = avatars[i];
                response.Body!.Add(new() { FileId = avatars[i].FileId, FileMetadata = avatars[i].File!.Set() });
            }

            return response;
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result<List<AvatarResponse>>.ErrorResult("DataBase error");
        }
    }
    public async Task<Result> RemoveAvatar(RemoveUserAvatarRequest request)
    {
        try
        {
            bool result = await _userAvatarRepository.TryDeleteAvatarByIds(request.UserId, request.FileId);

            return result ? Result.OkResult() : Result.BadRequest("Invalid id");
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("DataBase error");
        }
    }

    public async Task<Result> SetAvatar(SesUserAvatarRequest request)
    {

        try
        {
            UserAvatar avatar = new()
            {
                UserId = request.UserId,
                File = request.Metadata.Set()
            };

            await _userAvatarRepository.AddAsync(avatar);

            //_ = Task.Run(async () => await _chatAvatarPublisher.Publish(avatar));

            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("DataBase error");
        }
    }
}

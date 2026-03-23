using AdditionalHelpers.Services;
using CommonObjects.Requests.Avatars;
using CommonObjects.Responses;
using CommonObjects.Results;
using SecurityLibrary.Tokens;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;
using SpoofSettingsService.Setters;

namespace SpoofSettingsService.ServiceRealizations;

public class UserAvatarService(
        ILoggerService loggerService, 
        IUserAvatarRepository userAvatarRepository, 
        IUserAvatarValidator userAvatarValidator,
        IFileTokenService fileTokenService
    ) : IUserAvatarService
{
    private readonly IFileTokenService _fileTokenService = fileTokenService;
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IUserAvatarRepository _userAvatarRepository = userAvatarRepository;
    private readonly IUserAvatarValidator _userAvatarValidator = userAvatarValidator;

    public async Task<Result<AvatarResponse>> GetAvatar(GetUserAvatarRequest request, Guid userId)
    {
        try
        {
            UserAvatar? avatar = await _userAvatarRepository.GetActualUserAvatarById(request.UserId);
            Result result = _userAvatarValidator.IsAvailable(avatar);
            if (!result.Success)
                return Result<AvatarResponse>.From(result);

            return Result<AvatarResponse>.OkResult(
                new() { 
                    FileId = avatar!.Key2,
                    FileMetadata = avatar.File!.Set(avatar.OriginalFileName,
                        _fileTokenService.CreateToken(
                            userId,
                            avatar.Key2
                            )
                        ) 
                });
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result<AvatarResponse>.ErrorResult("DataBase error");
        }
    }

    public async Task<Result<List<AvatarResponse>>> GetAvatars(GetUserAvatarRequest request, Guid userId)
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
                response.Body!.Add(new() 
                { 
                    FileId = avatar.Key2,
                    FileMetadata = avatar.File!.Set(
                        avatar.OriginalFileName,
                        _fileTokenService.CreateToken(
                            userId,
                            avatar.Key2
                            )
                        ) 
                });
            }

            return response;
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result<List<AvatarResponse>>.ErrorResult("DataBase error");
        }
    }
    public async Task<Result> RemoveAvatar(RemoveUserAvatarRequest request, Guid userId)
    {
        try
        {
            bool result = await _userAvatarRepository.TryDeleteAvatarByIds(userId, request.FileId);

            return result ? Result.OkResult() : Result.BadRequest("Invalid id");
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("DataBase error");
        }
    }

    public async Task<Result> SetAvatar(SesUserAvatarRequest request, Guid userId)
    {

        try
        {
            if (!_fileTokenService.IsValid(request.Metadata.Token, userId, out Guid fileId))
                return Result.Forbidden("Invalid token");

            UserAvatar avatar = new()
            {
                Key1 = userId,
                File = request.Metadata.Set(fileId)
            };

            await _userAvatarRepository.AddAsync(avatar);
            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("DataBase error");
        }
    }
}

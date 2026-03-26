using AdditionalHelpers.Services;
using CommonObjects.Requests.Avatars;
using CommonObjects.Responses;
using CommonObjects.Results;
using RuleRoleHelper;
using SecurityLibrary;
using SecurityLibrary.Tokens;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;
using SpoofSettingsService.Setters;

namespace SpoofSettingsService.ServiceRealizations;

public class ChatAvatarService(
        ILoggerService loggerService,
        IChatAvatarRepository chatAvatarRepository,
        IChatAvatarValidator chatAvatarValidator,
        IRuleService ruleService,
        IFileTokenService fileTokenService
    ) : IChatAvatarService
{
    private readonly IFileTokenService _fileTokenService = fileTokenService;
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IChatAvatarRepository _chatAvatarRepository = chatAvatarRepository;
    private readonly IChatAvatarValidator _chatAvatarValidator = chatAvatarValidator;
    private readonly IRuleService _ruleService = ruleService;

    public async Task<Result<AvatarResponse>> GetAvatar(GetChatAvatarRequest request, Guid userId)
    {
        try
        {
            ChatAvatar? avatar = await _chatAvatarRepository.GetActualChatAvatarById(request.ChatId);
            Result result = _chatAvatarValidator.IsAvailable(avatar);
            if (!result.Success)
                return Result<AvatarResponse>.From(result);

            return Result<AvatarResponse>.OkResult(
                new()
                {
                    FileId = avatar!.FileId,
                    FileMetadata = avatar.File!.Set(
                        avatar.OriginalFileName,
                        _fileTokenService.CreateToken(userId, avatar.FileId),
                        Hasher.GetKey(avatar.FileId.ToByteArray()))
                });
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result<AvatarResponse>.ErrorResult("DataBase error");
        }
    }

    public async Task<Result<List<AvatarResponse>>> GetAvatars(GetChatAvatarRequest request, Guid userId)
    {
        try
        {
            List<ChatAvatar>? avatars = await _chatAvatarRepository.GetChatAvatarsById(request.ChatId);
            Result result = _chatAvatarValidator.IsAvailableCollection(avatars);
            if (!result.Success)
                return Result<List<AvatarResponse>>.From(result);

            ChatAvatar avatar = null!;
            List<AvatarResponse> response = [];

            for (int i = 0; i < avatars!.Count; i++)
            {
                avatar = avatars[i];
                response.Add(
                    new()
                    {
                        FileId = avatar.FileId,
                        FileMetadata = avatar.File!.Set(
                            avatar.OriginalFileName,
                            _fileTokenService.CreateToken(userId, avatar.FileId),
                            Hasher.GetKey(avatar.FileId.ToByteArray()))
                    });
            }

            return Result<List<AvatarResponse>>.OkResult(response);
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result<List<AvatarResponse>>.ErrorResult("DataBase error");
        }
    }

    public async Task<Result> RemoveAvatar(RemoveChatAvatarRequest request, Guid userId)
    {
        try
        {
            Result ruleResult = await _ruleService.HasPermissionAsync(
                    userId,
                    request.ChatId,
                    Permissions.DeleteAvatar
                );
            if (!ruleResult.Success)
                return ruleResult;

            bool result = await _chatAvatarRepository.TryDeleteAvatarByIds(request.ChatId, request.FileId);

            return result ? Result.OkResult() : Result.BadRequest("Invalid id");
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("DataBase error");
        }
    }

    public async Task<Result> SetAvatar(SetChatAvatarRequest request, Guid userId)
    {
        try
        {
            Result ruleResult = await _ruleService.HasPermissionAsync(
                    userId,
                    request.ChatId,
                    Permissions.ChangeAvatar
                );
            if (!ruleResult.Success)
                return ruleResult;
            if (!_fileTokenService.IsValid(request.Metadata.Token, userId, out Guid fileId))
                return Result.Forbidden("Invalid token");

            ChatAvatar chatAvatar = new()
            {
                ChatId = request.ChatId,
                OriginalFileName = request.Metadata.OriginalName,
                FileId = fileId,
            };
            chatAvatar.File.Id = request.FileId;

            await _chatAvatarRepository.AddAsync(chatAvatar);

            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("DataBase error");
        }
    }
}

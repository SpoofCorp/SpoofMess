using AdditionalHelpers.Services;
using CommonObjects.Requests.Avatars;
using CommonObjects.Responses;
using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.MessageBrokers;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;
using SpoofSettingsService.Setters;

namespace SpoofSettingsService.ServiceRealizations;
public class ChatAvatarService(ILoggerService loggerService, IChatAvatarFileService chatAvatarFileService, IChatAvatarRepository chatAvatarRepository, IChatAvatarValidator chatAvatarValidator) : IChatAvatarService
{
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IChatAvatarFileService _chatAvatarFileService = chatAvatarFileService;
    private readonly IChatAvatarRepository _chatAvatarRepository = chatAvatarRepository;
    private readonly IChatAvatarValidator _chatAvatarValidator = chatAvatarValidator;

    public async Task<Result<AvatarResponse>> GetAvatar(GetChatAvatarRequest request)
    {
        try
        {
            ChatAvatar? avatar = await _chatAvatarRepository.GetActualChatAvatarById(request.ChatId);
            Result result = _chatAvatarValidator.IsAvailable(avatar);
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

    public async Task<Result<List<AvatarResponse>>> GetAvatars(GetChatAvatarRequest request)
    {
        try
        {

            List<ChatAvatar>? avatars = await _chatAvatarRepository.GetChatAvatarsById(request.ChatId);
            Result result = _chatAvatarValidator.IsAvailableCollection(avatars);
            if (!result.Success)
                return Result<List<AvatarResponse>>.From(result);

            ChatAvatar avatar = null!;
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

    public async Task<Result> RemoveAvatar(RemoveChatAvatarRequest request)
    {
        try
        {
            bool result = await _chatAvatarRepository.TryDeleteAvatarByIds(request.ChatId, request.FileId);
            await _chatAvatarFileService.Delete(new() { FileId = request.FileId });
            return result ? Result.OkResult() : Result.BadRequest("Invalid id");
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("DataBase error");
        }
    }

    public async Task<Result> SetAvatar(SetChatAvatarRequest request)
    {
        try
        {
            ChatAvatar chatAvatar = new()
            {
                ChatId = request.ChatId,
                File = request.Metadata.Set()
            };
            chatAvatar.File.Id = request.FileId;

            await _chatAvatarRepository.AddAsync(chatAvatar);
            
            await _chatAvatarFileService.Create(new() { FileId = chatAvatar.File.Id });

            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("DataBase error");
        }
    }
}

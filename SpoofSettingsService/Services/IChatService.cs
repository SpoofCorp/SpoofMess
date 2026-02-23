using CommonObjects.Requests;
using CommonObjects.Requests.Changes;
using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services;

public interface IChatService
{
    public ValueTask<Result> ChangeSettings(
        ChangeChatSettingsRequest request,
        Guid userId
        );

    public ValueTask<Result> CreateChat(
        CreateChatRequest request, 
        Guid userId
        );

    public ValueTask<Result> DeleteChat(
        Guid chatId, 
        Guid userId
        );

    public Task<Result<ChatWithOwner>> GetChatWithOwner(
        Guid userId,
        Guid chatId
        );

    [Obsolete("Need check chat visibility and check user attitute to chat")]
    public Task<Result<Chat>> Get(
        Guid chatId
        );
}

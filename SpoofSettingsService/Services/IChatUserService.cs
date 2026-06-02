using CommonObjects.DTO;
using CommonObjects.Requests.ChatUsers;
using CommonObjects.Requests.Members;
using CommonObjects.Results;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services;

public interface IChatUserService
{
    public Task<Result<List<ChatMemberDTO>>> GetChatUsers(
            Guid userId,
            Guid chatId,
            string? lastLogin,
            DateTime? before,
            int count = 10
        );

    [Obsolete("Not check chat settings")]
    public Task<Result> Join(
            JoinToChatRequest request, 
            Guid userId
        );
    public Task<Result> Add(
            AddMemberRequest request,
            Guid userId
        );
    public Task<Result> Remove(
            DeleteMemberRequest request,
            Guid userId
        );
    public Task<Result<ChatUser>> Get(
            GetChatUserRequest request,
            Guid userId
        );
    public Task<Result<List<ChatUserDTO>>> GetUserChats(
            Guid userId, 
            DateTime before
        );
}
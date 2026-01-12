using AdditionalHelpers.Services;
using CommonObjects.Requests;
using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations;

public class ChatUserService(ILoggerService loggerService, IChatService chatService, IBaseValidator baseValidator, IRoleService roleTypeService, IUserRepository userRepository, IChatUserRepository chatUserRepository) : IChatUserService
{
    private readonly IBaseValidator _baseValidator = baseValidator;
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IChatService _chatService = chatService;
    private readonly IRoleService _roleTypeService = roleTypeService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IChatUserRepository _chatUserRepository = chatUserRepository;
    public async Task<Result> AddMember(AddMemberRequest request, Guid userId)
    {
        try
        {
            UserChatResult result = await _chatService.GetUserAndChat(userId, request.ChatId);
            if (!result.Result.Success)
                return result.Result;

            User? member = await _userRepository.GetByIdAsync(request.MemberId);
            Result validateResult = _baseValidator.ValidateExist(member);
            if (!validateResult.Success) return validateResult;

            Role? role = await _roleTypeService.GetRoleById(request.RoleId); 
            validateResult = _baseValidator.ValidateExist(role);
            if (!validateResult.Success) return validateResult;

            ChatUser newMember = new()
            {
                ChatId = result.User!.Id,
                UserId = member!.Id,
                RoleId = role!.Id,
                JoinedAt = DateTime.UtcNow,
            };

            await _chatUserRepository.AddAsync(newMember);
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("DataBase error");
        }
    }

    public Task<Result> ChangeMemberRights()
    {
        throw new NotImplementedException();
    }

    public async Task<Result> RemoveMember(DeleteMemberRequest request, Guid userId)
    {
        try
        {
            UserChatResult result = await _chatService.GetUserAndChat(userId, request.ChatId);
            if (!result.Result.Success)
                return result.Result;

            await _chatUserRepository.DeleteMemberById(request.MemberId, userId);
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("DataBase error");
        }
    }
}

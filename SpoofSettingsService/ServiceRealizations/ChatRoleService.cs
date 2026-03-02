using AdditionalHelpers.Services;
using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations;

public class ChatRoleService(
    IChatRoleRepository chatRoleRepository,
    IChatRoleValidator chatRoleValidator, 
    ILoggerService loggerService
    ) : IChatRoleService
{
    private readonly IChatRoleRepository _chatRoleRepository = chatRoleRepository;
    private readonly IChatRoleValidator _chatRoleValidator = chatRoleValidator;
    private readonly ILoggerService _loggerService = loggerService;

    public async Task<Result<ChatRole>> Get(long id)
    {
        try
        {
            ChatRole? chatRole = await _chatRoleRepository.GetByIdAsync(id);
            Result result = _chatRoleValidator.IsAvailable(chatRole);
            if (!result.Success)
                return Result<ChatRole>.From(result);

            return Result<ChatRole>.OkResult(chatRole!);
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<ChatRole>.ErrorResult("Database error");
        }
    }
}

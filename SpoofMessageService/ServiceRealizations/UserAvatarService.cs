using AdditionalHelpers.Services;
using CommonObjects.Results;
using CommunicationLibrary.Communication;
using SpoofMessageService.Services;
using SpoofMessageService.Services.Repositories;

namespace SpoofMessageService.ServiceRealizations;

public class UserAvatarService(
    IUserRepository userRepository,
    ILoggerService loggerService
    ) : IUserAvatarService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILoggerService _loggerService = loggerService;

    public async Task<Result> Create(CreateUserAvatar createUserAvatar)
    {
        try
        {
            return await _userRepository.ExecuteUpdateAvatar(
                createUserAvatar.UserId, 
                createUserAvatar.FileId, 
                createUserAvatar.OriginalFileName)
                    ? Result.OkResult()
                    : Result.BadRequest("Invalid id");
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result.ErrorResult("Database error");
        }
    }
}
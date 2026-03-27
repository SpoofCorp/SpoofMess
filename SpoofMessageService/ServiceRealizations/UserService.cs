using AdditionalHelpers.Services;
using CommonObjects.Results;
using CommunicationLibrary.Communication;
using SpoofMessageService.Models;
using SpoofMessageService.Services;
using SpoofMessageService.Services.Repositories;

namespace SpoofMessageService.ServiceRealizations;

public class UserService(
    IUserRepository userRepository,
    ILoggerService loggerService
    ) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILoggerService _loggerService = loggerService;

    public async Task<Result> Create(CreateUser createUser)
    {
        try
        {
            User user = new()
            {
                Name = createUser.Name,
                Login = createUser.Login,
                Id = createUser.UserId
            };
            await _userRepository.AddAsync(user);

            return Result.OkResult();
        }
        catch(Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result.ErrorResult("Database error");
        }
    }

    public async Task<Result> Delete(
            Guid userId
        )
    {
        try
        {
            return await _userRepository.DeleteById(userId)
                ? Result.OkResult()
                : Result.BadRequest("Invalid id");
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result.ErrorResult("Database error");
        }
    }

    public Task<Result> Update()
    {
        throw new NotImplementedException();
    }

    public async Task<Result> ChangeConnectionState(Guid userId, bool state)
    {
        try
        { 
            return await _userRepository.ExecuteUpdateConnection(userId, state)
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

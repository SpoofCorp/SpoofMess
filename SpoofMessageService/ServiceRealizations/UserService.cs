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

    public Task<Result> Delete()
    {
        throw new NotImplementedException();
    }

    public Task<Result> Update()
    {
        throw new NotImplementedException();
    }
}
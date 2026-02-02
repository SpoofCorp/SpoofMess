using AdditionalHelpers.Services;
using CommonObjects.Requests.Changes;
using CommonObjects.Results;
using CommunicationLibrary.Communication;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.MessageBrokers;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;
using SpoofSettingsService.Setters;

namespace SpoofSettingsService.ServiceRealizations;

public class UserService(ILoggerService logger, IUserRepository userRepository, IUserValidator userValidator, IUserMessageBrokerService userMessageBrokerService) : IUserService
{
    private readonly ILoggerService _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserValidator _userValidator = userValidator;
    private readonly IUserMessageBrokerService _userMessageBrokerService = userMessageBrokerService;

    public async Task<Result<User>> Get(Guid id)
    {
        try
        {
            User? user = await _userRepository.GetByIdAsync(id);
            Result result = _userValidator.IsAvailable(user);

            if (!result.Success) return Result<User>.From(result);

            return Result<User>.OkResult(user!);
        }
        catch (Exception ex)
        {
            _logger.Error("Database error", ex);
            return Result<User>.ErrorResult("Database error");
        }
    }

    public async Task<Result> ChangeSettings(ChangeUserSettingsRequest request, Guid userId)
    {
        try
        {
            User? user = await _userRepository.GetByIdAsync(userId);
            Result result = _userValidator.IsAvailable(user);

            if (!result.Success) return result;

            user!.Set(request);
            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _logger.Error("Database error", ex);
            return Result.ErrorResult("Database error");
        }
    }

    public async Task<Result> Delete(Guid userId)
    {
        try
        {
            User? user = await _userRepository.GetByIdAsync(userId);
            Result result = _userValidator.IsAvailable(user);

            if (!result.Success) return result;
            user!.IsDeleted = true;

            await _userRepository.SoftDeleteAsync(user);
            await _userMessageBrokerService.ConfirmDelete(new(userId, ""));

            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _logger.Error("Database error", ex);
            return Result.ErrorResult("Database error");
        }
    }

    public async Task<Result> Create(CreateUser createUser)
    {
        try
        {
            User user = new()
            {
                Id = createUser.UserId,
                Name = createUser.Name,
            };
            await _userRepository.AddAsync(user);
            await _userMessageBrokerService.ConfirmCreate(createUser);
            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _logger.Error("Database error", ex);
            return Result.ErrorResult("Database error");
        }
    }
}
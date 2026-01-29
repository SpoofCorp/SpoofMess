using AdditionalHelpers.Services;
using CommonObjects.Requests;
using CommonObjects.Results;
using CommunicationLibrary.Services;
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

    public async ValueTask<Result> ChangeSettings(ChangeUserSettingsRequest request, Guid userId)
    {
        try
        {
            User? user = await _userRepository.GetByIdAsync(userId);
            Result result = _userValidator.IsAvailable(user);

            if (!result.Success) return result;

            user!.Set(request);
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            _logger.Error("Database error", ex);
            return Result.ErrorResult("Database error");
        }
    }

    public async ValueTask<Result> Delete(Guid userId)
    {
        try
        {
            User? user = await _userRepository.GetByIdAsync(userId);
            Result result = _userValidator.IsAvailable(user);

            if (!result.Success) return result;

            user!.IsDeleted = true;
            await _userRepository.SoftDeleteAsync(user);
            await _userMessageBrokerService.ConfirmDelete(new() { UserId = userId });
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            _logger.Error("Database error", ex);
            return Result.ErrorResult("Database error");
        }
    }

    public async ValueTask<Result> Create(Guid userId)
    {
        try
        {
            User user = new()
            {
                Id = userId,
                Name = string.Empty
            };
            await _userRepository.AddAsync(user);
            await _userMessageBrokerService.ConfirmCreate(new() { UserId = userId });
            return Result.SuccessResult();
        }
        catch (Exception ex)
        {
            _logger.Error("Database error", ex);
            return Result.ErrorResult("Database error");
        }
    }
}
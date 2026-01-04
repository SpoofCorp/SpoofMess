using CommonObjects.Requests;
using CommonObjects.Results;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Interfaces;
using SpoofSettingsService.Services.Validators;
using SpoofSettingsService.Setter;

namespace SpoofSettingsService.ServiceRealizations;

public class UserService(IUserRepository userRepository, IUserValidator userValidator) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserValidator _userValidator = userValidator;

    public async ValueTask<Result> ChangeSettings(ChangeUserSettingsRequest request, Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        Result result = _userValidator.Validate(user);

        if (!result.Success) return result;

        user!.Set(request);
        return Result.SuccessResult();
    }

    public async ValueTask<Result> Delete(Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        Result result = _userValidator.Validate(user);

        if (!result.Success) return result;

        user!.IsDeleted = true;
        await _userRepository.SoftDeleteAsync(user);

        return Result.SuccessResult();
    }
}
using AdditionalHelpers.Services;
using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations;

public class RoleService(ILoggerService loggerService, IRoleRepository roleRepository, IRoleValidator roleValidator) : IRoleService
{
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly IRoleValidator _roleValidator = roleValidator;

    public async Task<Result<Role>> GetRoleById(int roleId)
    {
        try
        {
            Role? role = await _roleRepository.GetByIdAsync(roleId);
            Result result = _roleValidator.IsAvailable(role);
            if (!result.Success) return Result<Role>.From(result);

            return Result<Role>.OkResult(role!);
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result<Role>.ErrorResult("DataBase error");
        }
    }
}

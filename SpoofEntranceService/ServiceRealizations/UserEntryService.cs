using AdditionalHelpers.Services;
using CommonObjects.Requests;
using CommonObjects.Responses;
using CommonObjects.Results;
using SecurityLibrary;
using SpoofEntranceService.Models;
using SpoofEntranceService.Services;
using SpoofEntranceService.Services.Repositories;
using SpoofEntranceService.Services.Validators;

namespace SpoofEntranceService.ServiceRealizations;

public class UserEntryService(IUserEntryRepository repository, IUserPublisherService userPublisherService, IUserEntryValidator validator, ILoggerService logService, ITokenService tokenService, ISessionService sessionService) : IUserEntryService
{
    private readonly IUserPublisherService _userPublisherService = userPublisherService;
    private readonly IUserEntryRepository _repository = repository;
    private readonly ISessionService _sessionService = sessionService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IUserEntryValidator _validator = validator;
    private readonly ILoggerService _logService = logService;

    public async Task<Result<UserAuthorizeResponse>> Authorization(HttpContext context,
        UserAuthorizeRequest request,
        SessionInfo sessionInfo)
    {
        try
        {
            UserEntry? user = await _repository.GetByLogin(request.Login);

            Result result = _validator.IsActive(user);
            if (!result.Success)
                return Result<UserAuthorizeResponse>.From(result);

            if (!Hasher.VerifyPassword(request.Password, user!.PasswordHash))
                return Result<UserAuthorizeResponse>.ErrorResult("Invalid password", 403);

            await _sessionService.StartSession(context, user, sessionInfo);

            Result<UserAuthorizeResponse> response = await _tokenService.Create(sessionInfo);
            sessionInfo.UserEntry = user;
            return response;
        }
        catch (Exception ex)
        {
            _logService.Error("Error", ex);
            return Result<UserAuthorizeResponse>.ErrorResult(ex.Message);
        }
    }
    public async Task<Result<UserAuthorizeResponse>> Registration(HttpContext context, RegistrationRequest request, SessionInfo sessionInfo)
    {
        try
        {
            UserEntry? user = await _repository.GetByLogin(request.Login);

            if (user is { IsDeleted: false })
                return Result<UserAuthorizeResponse>.ErrorResult("Login is busy");

            UserEntry newUser = new()
            {
                Id = Guid.CreateVersion7(),
                UniqueName = request.Login,
                PasswordHash = Hasher.HashPassword(request.Password),
            };

            newUser.UserEntryOperationStatuses.Add(new()
            {
                IsActual = true,
                OperationStatusId = (short)OperationsStatus.Pending,
            });

            await _repository.Change(newUser, user);

            await _sessionService.StartSession(context, newUser, sessionInfo);

            await _userPublisherService.Create(new() { UserId = newUser.Id });
            return await _tokenService.Create(sessionInfo);
        }
        catch (Exception ex)
        {
            _logService.Error("Error", ex);
            return Result<UserAuthorizeResponse>.ErrorResult(ex.Message);
        }
    }

    public async Task<Result> Delete(SessionInfo sessionInfo)
    {
        try
        {
            UserEntry? user = await _repository.GetByIdAsync(sessionInfo.UserEntryId);

            Result result = _validator.IsActive(user);
            if (!result.Success)
                return result;

            await _sessionService.EndSessions(sessionInfo.Id, true);
            await _repository.SoftDeleteAsync(user!);
            return Result.OkResult("Ok");
        }
        catch(Exception ex)
        {
            _logService.Error("Error", ex);
            return Result.ErrorResult(ex.Message);
        }
    }
    public async Task Confirm(Guid userId) =>
        await ChangeStatus(userId, false);

    public async Task Error(Guid userId) =>
        await ChangeStatus(userId, true);
    public async Task Delete(Guid userId)
    {
        await ChangeStatus(userId, true);
        await _userPublisherService.Delete(new(userId));
    }

    public async Task ChangeStatus(Guid userId, bool isDeleted)
    {
        try
        {
            UserEntry? user = await _repository.GetByIdAsync(userId);

            Result result = _validator.IsActive(user);
            if (!result.Success)
                return;
            user!.IsDeleted = isDeleted;
            user.UserEntryOperationStatuses.Add(new()
            {
                IsActual = true,
                OperationStatusId = (short)OperationsStatus.Success,
                TimeSet = DateTime.UtcNow
            });
            await _repository.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            _logService.Error("Error", ex);
        }
    }
}
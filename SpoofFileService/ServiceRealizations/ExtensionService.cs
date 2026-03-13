using AdditionalHelpers.Services;
using CommonObjects.Results;
using SpoofFileInfo;
using SpoofFileService.Models;
using SpoofFileService.Services;
using SpoofFileService.Services.Repositories;
using SpoofFileService.Services.Validators;

namespace SpoofFileService.ServiceRealizations;

public class ExtensionService(
    IExtensionRepository extensionRepository,
    IExtensionValidator extensionValidator,
    IFileClassifier fileClassifier,
    ILoggerService loggerService) : IExtensionService
{
    private readonly IFileClassifier _fileClassifier = fileClassifier;
    private readonly IExtensionValidator _extensionValidator = extensionValidator;
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IExtensionRepository _extensionRepository = extensionRepository;

    public async Task<Result<Extension>> Get(FileExtension2 fileExtension2)
    {
        try
        {
            Extension? extension = await _extensionRepository.GetByName(fileExtension2);
            Result result = _extensionValidator.IsAvailable(extension);
            if (!result.Success)
                return Result<Extension>.From(result);
            return Result<Extension>.OkResult(extension!);
        }
        catch(Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<Extension>.InternalServerError();
        }
    }

    public async Task<Result<Extension>> GetByFile(string filePath)
    {
        try
        {
            return await GetByName(_fileClassifier.GetExtension(filePath));
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<Extension>.InternalServerError();
        }
    }

    public async Task<Result<Extension>> GetByName(FileExtension2 fileExtension2)
    {
        try
        {
            Extension? extension = await _extensionRepository.GetByName(fileExtension2);
            Result result = _extensionValidator.IsAvailable(extension);
            if (!result.Success)
                return Result<Extension>.From(result);
            return Result<Extension>.OkResult(extension!);
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<Extension>.InternalServerError();
        }
    }
}

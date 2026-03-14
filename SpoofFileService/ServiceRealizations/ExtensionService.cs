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

    public async Task<Result<ExtensionDto>> Get(FileExtension2 fileExtension2)
    {
        try
        {
            ExtensionDto? extension = await _extensionRepository.GetByName(fileExtension2);
            Result result = _extensionValidator.IsAvailable(extension);
            if (!result.Success)
                return Result<ExtensionDto>.From(result);
            return Result<ExtensionDto>.OkResult(extension!);
        }
        catch(Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<ExtensionDto>.InternalServerError();
        }
    }

    public async Task<Result<ExtensionDto>> GetByFile(string filePath)
    {
        try
        {
            return await GetByName(_fileClassifier.GetExtension(filePath));
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<ExtensionDto>.InternalServerError();
        }
    }

    public async Task<Result<ExtensionDto>> GetByName(FileExtension2 fileExtension2)
    {
        try
        {
            ExtensionDto? extension = await _extensionRepository.GetByName(fileExtension2);
            Result result = _extensionValidator.IsAvailable(extension);
            if (!result.Success)
                return Result<ExtensionDto>.From(result);
            return Result<ExtensionDto>.OkResult(extension!);
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<ExtensionDto>.InternalServerError();
        }
    }
}

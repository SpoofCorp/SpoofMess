using AdditionalHelpers.Services;
using CommonObjects.Results;
using SpoofFileParser;
using SpoofFileParser.FileMetadata;
using SpoofFileService.Models;
using SpoofFileService.Services;
using SpoofFileService.Services.Repositories;
using SpoofFileService.Services.Validators;

namespace SpoofFileService.ServiceRealizations;

public class ExtensionService(
    IExtensionRepository extensionRepository,
    IExtensionValidator extensionValidator,
    IFileClassifier fileClassifier,
    ISerializer serializer,
    ILoggerService loggerService) : IExtensionService
{
    private readonly IFileClassifier _fileClassifier = fileClassifier;
    private readonly IExtensionValidator _extensionValidator = extensionValidator;
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IExtensionRepository _extensionRepository = extensionRepository;
    private readonly ISerializer _serializer = serializer;

    public async Task<Result<ExtensionDto>> GetByFile(FileObject fileObject)
    {
        try
        {
            IFileMetadata? metadata = _fileClassifier.GetFileMetadata(fileObject.Path);
            if(metadata is null)
                return Result<ExtensionDto>.InternalServerError("Parser bad working");

            ExtensionDto? extension = await _extensionRepository.GetByName(metadata.Id, metadata.Extension, metadata.FileType.ToString());
            Result result = _extensionValidator.IsAvailable(extension);
            if (!result.Success)
                return Result<ExtensionDto>.From(result);
            fileObject.Metadata = _serializer.Serialize(metadata);
            fileObject.ExtensionId = metadata.Id;
            fileObject.Path = $"{fileObject.Id}.{metadata.Extension}";
            return Result<ExtensionDto>.OkResult(extension!);
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<ExtensionDto>.InternalServerError();
        }
    }

    public async Task<Result<ExtensionDto>> GetByName(FileExtension fileExtension2)
    {
        try
        {
            ExtensionDto? extension = await _extensionRepository.GetByName(fileExtension2.Id, fileExtension2.Name, fileExtension2.Type.ToString());
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

using AdditionalHelpers.Services;
using CommonObjects.Results;
using SpoofMessageService.Models;
using SpoofMessageService.Services;
using SpoofMessageService.Services.Repositories;
using SpoofMessageService.Services.Validators;

namespace SpoofMessageService.ServiceRealizations;

public class FileMetadatumService(IFileMetadatumRepository fileMetadatumRepository, ILoggerService loggerService, IFileMetadatumValidator fileMetadatumValidator) : IFileMetadatumService
{
    private readonly IFileMetadatumRepository _fileMetadatumRepository = fileMetadatumRepository;
    private readonly IFileMetadatumValidator _fileMetadatumValidator = fileMetadatumValidator;
    private readonly ILoggerService _loggerService = loggerService;
    public async Task<Result<FileMetadatum>> Get(Guid fileId)
    {
        try
        {
            FileMetadatum? fileMetadatum = await _fileMetadatumRepository.GetByIdAsync(fileId);
            Result result = _fileMetadatumValidator.IsAvailable(fileMetadatum);
            if (!result.Success)
                return Result<FileMetadatum>.From(result);
            return Result<FileMetadatum>.OkResult(fileMetadatum!);
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result<FileMetadatum>.InternalServerError();
        }
    }
}

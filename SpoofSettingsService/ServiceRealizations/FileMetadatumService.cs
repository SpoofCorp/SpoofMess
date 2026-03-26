using AdditionalHelpers.Services;
using CommonObjects.DTO;
using CommonObjects.Results;
using CommunicationLibrary.Communication;
using SecurityLibrary.Tokens;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations;

public class FileMetadatumService(
    IFileMetadatumRepository fileMetadatumRepository,
    IFileTokenService fileTokenService,
    IFileMetadatumValidator fileMetadatumValidator,
    ILoggerService loggerService) : IFileMetadatumService
{
    private readonly IFileMetadatumValidator _fileMetadatumValidator = fileMetadatumValidator;
    private readonly IFileMetadatumRepository _fileMetadatumRepository = fileMetadatumRepository;
    private readonly IFileTokenService _fileTokenService = fileTokenService;
    private readonly ILoggerService _loggerService = loggerService;

    public async Task<Result> Save(CreateFile createFile)
    {
        try
        {
            FileMetadatum? fileMetadatum = new()
            {
                Id = createFile.FileId,
                Size = createFile.Size,
                Category = createFile.Category,
                Metadata = createFile.Metadata,
            };
            await _fileMetadatumRepository.AddAsync(fileMetadatum);
            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.InternalServerError();
        }
    }

    public async Task<Result> Delete(DeleteFile deleteFile)
    {
        try
        {
            return await _fileMetadatumRepository.SoftDeleteAsync(deleteFile.FileId)
                ? Result.OkResult()
                : Result.BadRequest("Invalid id");
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.InternalServerError();
        }
    }

    public async Task<Result<FileMetadatum>> GetByToken(byte[] token, Guid userId, FileCategory type)
    {
        try
        {
            if (!_fileTokenService.IsValid(token, userId, out Guid fileId))
                return Result<FileMetadatum>.Forbidden("Invalid token");

            FileMetadatum? fileMetadatum = await _fileMetadatumRepository.GetByIdAsync(fileId);
            Result result = _fileMetadatumValidator.IsAvailable(fileMetadatum, type);
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

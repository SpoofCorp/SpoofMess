using AdditionalHelpers.Services;
using CommonObjects.Results;
using SpoofFileService.Models;
using SpoofFileService.ServiceRealizatoionss;
using SpoofFileService.ServiceRealizatoionss.Repositories;
using SpoofFileService.ServiceRealizatoionss.Validators;
using SpoofFileService.Services;

namespace SpoofFileService.ServiceRealizations;

public class FileService(ILoggerService loggerService, IFileRepository fileRepository, IFileValidator fileValidator, IFileWorkerService fileWorkerService) : IFileService
{
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IFileRepository _fileRepository = fileRepository;
    private readonly IFileValidator _fileValidator = fileValidator;
    private readonly IFileWorkerService _fileWorkerService = fileWorkerService;
    public async Task<Result> DeleteFile(Guid fileId)
    {
        try
        {
            FileObject? fileObject = await _fileRepository.GetByIdAsync(fileId);
            Result result = _fileValidator.IsAvailableAndFileExists(fileObject);
            if (!result.Success)
                return result;

            await _fileRepository.SoftDeleteAsync(fileObject!);
            await _fileWorkerService.Delete(fileObject!.FilePath);

            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result.ErrorResult("Internal server error");
        }
    }

    public async ValueTask<Result<FileStream>> GetFile(Guid fileId, Guid userId)
    {
        try
        {
            FileObject? fileObject = await _fileRepository.GetByIdAsync(fileId);
            Result result = _fileValidator.IsAvailable(fileObject);
            if (!result.Success)
                return Result<FileStream>.From(result);

            FileStream? stream = await _fileWorkerService.Get(fileObject!.FilePath);

            return stream is null ? Result<FileStream>.NotFoundResult("File is not found") : Result<FileStream>.OkResult(stream);
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<FileStream>.ErrorResult("Internal server error");
        }
    }

    public async Task<Result> SaveFile(IFormFile file, Guid? fileId)
    {
        try
        {
            if (file == null || file.Length == 0)
                return Result.BadRequest("No file uploaded");

            string filePath = await _fileWorkerService.Save(file);

            FileObject fileObject = new()
            {
                Id = fileId ?? Guid.CreateVersion7(),
                FilePath = filePath,
                IsDeleted = false,
                LastModified = DateTime.UtcNow,
            };

            await _fileRepository.AddAsync(fileObject);
            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result.ErrorResult("Internal server error");
        }
    }
}

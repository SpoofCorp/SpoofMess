using AdditionalHelpers.Services;
using CommonObjects.Results;
using SpoofFileService.Models;
using SpoofFileService.ServiceRealizatoionss;
using SpoofFileService.ServiceRealizatoionss.Repositories;
using SpoofFileService.ServiceRealizatoionss.Validators;

namespace SpoofFileService.ServiceRealizations;

public class FileService(ILoggerService loggerService, IFileRepository fileRepository, IFileValidator fileValidator) : IFileService
{
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IFileRepository _fileRepository = fileRepository;
    private readonly IFileValidator _fileValidator = fileValidator;
    public async Task<Result> DeleteFile(Guid fileId)
    {
        try
        {
            FileObject? fileObject = await _fileRepository.GetByIdAsync(fileId);
            Result result = _fileValidator.IsAvailableAndFileExists(fileObject);
            if (!result.Success)
                return result;

            await _fileRepository.SoftDeleteAsync(fileObject!);
            File.Delete(fileObject!.FilePath);

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

            return Result<FileStream>.OkResult(new(fileObject!.FilePath, FileMode.Open));
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

            string filePath = $"{Path.GetFileNameWithoutExtension(file.FileName)}{Guid.NewGuid()}.{Path.GetExtension(file.FileName)}";
            string directoryPath = $@"{Directory.GetCurrentDirectory()}\Storage\";
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            filePath = Path.Combine(directoryPath, filePath);
            FileObject fileObject = new()
            {
                Id = fileId ?? Guid.CreateVersion7(),
                FilePath = filePath,
                IsDeleted = false,
                LastModified = DateTime.UtcNow,
            };
            await _fileRepository.AddAsync(fileObject);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result.ErrorResult("Internal server error");
        }
    }
}

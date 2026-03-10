using AdditionalHelpers.Services;
using CommonObjects.Requests.Files;
using CommonObjects.Results;
using SecurityLibrary;
using SecurityLibrary.Tokens;
using SpoofFileService.Models;
using SpoofFileService.Services;
using SpoofFileService.Services.Repositories;
using SpoofFileService.Services.Validators;

namespace SpoofFileService.ServiceRealizations;

public class FileService(
    ILoggerService loggerService, 
    IFileRepository fileRepository, 
    IFileValidator fileValidator,
    IFileWorkerService fileWorkerService,
    IFingerprintService fingerprintService,
    IExtensionService extensionService,
    IFileTokenService fileTokenService) : IFileService
{
    private readonly IExtensionService _extensionService = extensionService;
    private readonly ILoggerService _loggerService = loggerService;
    private readonly IFileRepository _fileRepository = fileRepository;
    private readonly IFileValidator _fileValidator = fileValidator;
    private readonly IFileWorkerService _fileWorkerService = fileWorkerService;
    private readonly IFingerprintService _fingerprintService = fingerprintService;
    private readonly IFileTokenService _fileTokenService = fileTokenService;

    public async Task<Result> ExistL1(FingerprintExistL1L2 request)
    {
        try
        {
            bool exist = await _fileRepository.PreExistByL1AndL2(request);

            return exist 
                ? Result.BadRequest("Collision")
                : Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result.InternalServerError();
        }
    }

    public async Task<Result<byte[]>> ExistL3(FingerprintExistL3 request, Guid userId)
    {
        try
        {
            FileObject? fileObject = await _fileRepository.GetByL3(request.Fingerprint, request.Metadata);
            Result result = _fileValidator.IsAvailable(fileObject);
            return result.Success
                ? Result<byte[]>.OkResult(_fileTokenService.CreateToken(userId, fileObject!.Id))
                : Result<byte[]>.From(result);
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<byte[]>.InternalServerError();
        }
    }

    public async Task<Result> DeleteFile(byte[] fileId,
        FileMetadata Metadata)
    {
        try
        {
            FileObject? fileObject = await _fileRepository.GetByL3(fileId, Metadata);
            Result result = _fileValidator.IsAvailableAndFileExists(fileObject);
            if (!result.Success)
                return result;

            await _fileRepository.SoftDeleteAsync(fileObject!);
            await _fileWorkerService.Delete(fileObject!.Path);

            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result.InternalServerError();
        }
    }

    public async ValueTask<Result<FileStream>> GetFile(byte[] token, Guid userId)
    {
        try
        {
            if(!_fileTokenService.IsValid(token, userId, out Guid fileId))
                return Result<FileStream>.Forbidden("Invalid token");

            FileObject? fileObject = await _fileRepository.GetByIdAsync(fileId);
            Result result = _fileValidator.IsAvailable(fileObject);
            if (!result.Success)
                return Result<FileStream>.From(result);

            FileStream? stream = await _fileWorkerService.Get(fileObject!.Path);

            return stream is null ? Result<FileStream>.NotFoundResult("File is not found") : Result<FileStream>.OkResult(stream);
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<FileStream>.InternalServerError();
        }
    }

    public async Task<Result<byte[]>> SaveFile(IFormFile file, Guid userId)
    {
        try
        {
            if (file == null || file.Length == 0)
                return Result<byte[]>.BadRequest("No file uploaded");
            FileObject fileObject;
            string firstPath;
            Result<Extension> fileExtension;
            Guid fileId = Guid.CreateVersion7();
            if (file.Length > 50 * 1024 * 1024)
            {
                Result<FingerprintFull> fingerprintResult = await _fingerprintService.GetFull(file);
                if (!fingerprintResult.Success)
                    return Result<byte[]>.From(fingerprintResult);

                firstPath = fingerprintResult.Body.FileResult.FilePath;
                fileExtension = await _extensionService.GetByFile(firstPath);
                if (!fileExtension.Success)
                    return Result<byte[]>.From(fileExtension);
                fileObject = new()
                {
                    Id = fileId,
                    Path = fileId.ToString(),
                    IsDeleted = false,
                    LastModified = DateTime.UtcNow,
                    L1 = fingerprintResult.Body!.L1,
                    L2 = fingerprintResult.Body!.L2,
                    L3 = fingerprintResult.Body!.FileResult.Fingerprint,
                    ExtensionId = fileExtension.Body!.Id
                };
            }
            else
            {
                Result<FileResult> fingerprintResult = await _fingerprintService.GetOnlyFullFingerprint(file);
                if (!fingerprintResult.Success)
                    return Result<byte[]>.From(fingerprintResult);
                firstPath = fingerprintResult.Body.FilePath;
                fileExtension = await _extensionService.GetByFile(firstPath);
                if(!fileExtension.Success)
                    return Result<byte[]>.From(fileExtension);
                fileObject = new()
                {
                    Id = fileId,
                    L3 = fingerprintResult.Body!.Fingerprint,
                    Path = fileId.ToString(),
                    IsDeleted = false,
                    LastModified = DateTime.UtcNow,
                    ExtensionId = fileExtension.Body!.Id
                };
            }

            if (!await _fileRepository.Save(fileObject))
                await _fileWorkerService.Move(firstPath, fileObject.Path);

            return Result<byte[]>.OkResult(_fileTokenService.CreateToken(userId, fileObject.Id));
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result<byte[]>.InternalServerError();
        }
    }
}

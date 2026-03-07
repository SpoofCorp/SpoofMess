using AdditionalHelpers.Services;
using CommonObjects.Requests.Files;
using CommonObjects.Results;
using SecurityLibrary;
using SecurityLibrary.Tokens;
using SpoofFileService.Models;
using SpoofFileService.Services;
using SpoofFileService.Services.Repositories;
using SpoofFileService.Services.Validators;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SpoofFileService.ServiceRealizations;

public class FileService(
    ILoggerService loggerService, 
    IFileRepository fileRepository, 
    IFileValidator fileValidator,
    IFileWorkerService fileWorkerService,
    IFingerprintService fingerprintService,
    IFileTokenService fileTokenService) : IFileService
{
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
            FileObject? fileObject = await _fileRepository.GetByIdAsync(request.Fingerprint);
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

    public async Task<Result> DeleteFile(byte[] fileId)
    {
        try
        {
            FileObject? fileObject = await _fileRepository.GetByIdAsync(fileId);
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
            if(!_fileTokenService.IsValid(token, userId, out SecurityLibrary.Tokens.FileAccess access))
                return Result<FileStream>.Forbidden("Invalid token");
            byte[] l3 = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<L3, byte>(ref access.L3), 32).ToArray();
            FileObject? fileObject = await _fileRepository.GetByIdAsync(l3);
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
            if (file.Length > 50 * 1024 * 1024)
            {
                Result<FingerprintFull> fingerprintResult = await _fingerprintService.GetFull(file);
                if (!fingerprintResult.Success)
                    return Result<byte[]>.From(fingerprintResult);
                fileObject = new()
                {
                    Id = fingerprintResult.Body!.FileResult.Fingerprint,
                    Path = Convert.ToHexString(fingerprintResult.Body!.FileResult.Fingerprint),
                    IsDeleted = false,
                    LastModified = DateTime.UtcNow,
                    L1 = fingerprintResult.Body!.L1,
                    L2 = fingerprintResult.Body!.L2,
                    CategoryId = 1,
                    ExtensionId = 1
                };
                firstPath = fingerprintResult.Body.FileResult.FilePath;
            }
            else
            {
                Result<FileResult> fingerprintResult = await _fingerprintService.GetOnlyFullFingerprint(file);
                if (!fingerprintResult.Success)
                    return Result<byte[]>.From(fingerprintResult);
                fileObject = new()
                {
                    Id = fingerprintResult.Body!.Fingerprint,
                    Path = Convert.ToHexString(fingerprintResult.Body!.Fingerprint),
                    IsDeleted = false,
                    LastModified = DateTime.UtcNow,
                    CategoryId = 1,
                    ExtensionId = 1
                };
                firstPath = fingerprintResult.Body.FilePath;
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

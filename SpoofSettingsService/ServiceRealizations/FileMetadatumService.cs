using AdditionalHelpers.Services;
using CommonObjects.Results;
using CommunicationLibrary.Communication;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations;

public class FileMetadatumService(
    IFileMetadatumRepository fileMetadatumRepository, 
    ILoggerService loggerService) : IFileMetadatumService
{
    private readonly IFileMetadatumRepository _fileMetadatumRepository = fileMetadatumRepository;
    private readonly ILoggerService _loggerService = loggerService;

    public async Task<Result> Save(CreateFile createFile)
    {
        try
        {
            FileMetadatum? fileMetadatum = new()
            {
                Id = createFile.FileId,
                Size = createFile.Size,
                Extension = createFile.Extension,
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
}

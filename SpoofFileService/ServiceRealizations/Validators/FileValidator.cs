using CommonObjects.Results;
using DataSaveHelpers.ServiceRealizations;
using SpoofFileService.Models;
using SpoofFileService.Services.Validators;

namespace SpoofFileService.ServiceRealizations.Validators;

public class FileValidator : SoftDeletableValidator<FileObject>, IFileValidator
{
    public Result IsFound(FileObject? file)
    {
        if (file is null)
            return Result.NotFoundResult("File not found");
        return Result.OkResult();
    }
    public Result IsAvailableAndFileExists(FileObject? obj)
    {
        Result result = IsAvailable(obj);
        if (!result.Success)
            return result;

        if (File.Exists(obj!.Path))
            return Result.OkResult();

        return Result.BadRequest("File is not exist");
    }
}

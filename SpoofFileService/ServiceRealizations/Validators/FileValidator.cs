using CommonObjects.Results;
using DataSaveHelpers.ServiceRealizations;
using SpoofFileService.Models;
using SpoofFileService.ServiceRealizatoionss.Validators;

namespace SpoofFileService.ServiceRealizations.Validators;

public class FileValidator : SoftDeletableValidator<FileObject>, IFileValidator
{
    public Result IsAvailableAndFileExists(FileObject? obj)
    {
        Result result = IsAvailable(obj);
        if (!result.Success)
            return result;

        if (File.Exists(obj!.FilePath))
            return Result.OkResult();

        return Result.BadRequest("File is not exist");
    }
}

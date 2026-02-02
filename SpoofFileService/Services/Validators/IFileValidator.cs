using CommonObjects.Results;
using DataSaveHelpers.Services;
using SpoofFileService.Models;

namespace SpoofFileService.Services.Validators;

public interface IFileValidator : ISoftDeletableValidator<FileObject>
{
    public Result IsAvailableAndFileExists(FileObject? fileObject);
}

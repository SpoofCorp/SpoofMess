using CommonObjects.Results;
using DataSaveHelpers.Services;
using SpoofFileService.Models;

namespace SpoofFileService.Services.Validators;

public interface IFileValidator : ISoftDeletableValidator<FileObject>
{
    public Result IsFound(FileObject? file);
    public Result IsAvailableAndFileExists(FileObject? fileObject);
}

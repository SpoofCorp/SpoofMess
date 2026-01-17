using CommonObjects.Results;
using DataSaveHelpers.Services;
using SpoofFileService.Models;

namespace SpoofFileService.ServiceRealizatoionss.Validators;

public interface IFileValidator : ISoftDeletableValidator<FileObject>
{
    public Result IsAvailableAndFileExists(FileObject? fileObject);
}

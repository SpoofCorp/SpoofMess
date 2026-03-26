using CommonObjects.DTO;
using CommonObjects.Results;
using DataSaveHelpers.Services;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services.Validators;

public interface IFileMetadatumValidator : ISoftDeletableValidator<FileMetadatum>
{
    public Result IsAvailable(
        FileMetadatum? fileMetadatum,
        FileCategory category);
}

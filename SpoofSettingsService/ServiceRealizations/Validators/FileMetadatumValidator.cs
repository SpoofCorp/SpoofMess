using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Validators;
using DataSaveHelpers.ServiceRealizations;
using CommonObjects.DTO;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class FileMetadatumValidator : SoftDeletableValidator<FileMetadatum>, IFileMetadatumValidator
{
    public Result IsAvailable(FileMetadatum? fileMetadatum, FileCategory category)
    {
        Result result = IsAvailable(fileMetadatum);
        if (!result.Success)
            return result;

        if (!fileMetadatum!.Category.Equals(category.ToString(), StringComparison.InvariantCultureIgnoreCase))
            return Result.Forbidden($"Invalid file category, needed: {category}");

        return Result.OkResult();
    }
}

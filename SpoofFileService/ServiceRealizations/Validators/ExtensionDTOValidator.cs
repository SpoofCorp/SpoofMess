using CommonObjects.Results;
using SpoofFileService.Models;
using SpoofFileService.Services.Validators;

namespace SpoofFileService.ServiceRealizations.Validators;

public class ExtensionDTOValidator : IExtensionValidator
{
    public Result IsAvailable(ExtensionDto extensionDto)
    {
        if (extensionDto is null)
            return Result.BadRequest("Invalid extension");
        else 
            return Result.OkResult();
    }
}

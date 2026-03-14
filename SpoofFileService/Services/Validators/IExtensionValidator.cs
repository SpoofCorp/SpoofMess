using CommonObjects.Results;
using SpoofFileService.Models;

namespace SpoofFileService.Services.Validators;

public interface IExtensionValidator
{
    public Result IsAvailable(ExtensionDto extensionDto);
}

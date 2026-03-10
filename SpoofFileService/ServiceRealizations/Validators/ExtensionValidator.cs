using DataSaveHelpers.ServiceRealizations;
using SpoofFileService.Models;
using SpoofFileService.Services.Validators;

namespace SpoofFileService.ServiceRealizations.Validators;

public class ExtensionValidator : SoftDeletableValidator<Extension>, IExtensionValidator
{

}

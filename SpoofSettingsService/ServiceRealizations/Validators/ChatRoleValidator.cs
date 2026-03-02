using DataSaveHelpers.ServiceRealizations;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Validators;

namespace SpoofSettingsService.ServiceRealizations.Validators;

public class ChatRoleValidator : SoftDeletableValidator<ChatRole>, IChatRoleValidator
{
}

using DataSaveHelpers.ServiceRealizations.Repositories.WithCache;
using DataSaveHelpers.Services;
using SpoofSettingsService.Models;
using SpoofSettingsService.ServiceRealizations.Validators;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class ChatUserRuleRepository(ICacheService cache, IChatUserRuleValidator chatUserRuleValidator, SpoofSettingsServiceContext context, IProcessQueueTasksService tasksService) : CachedSoftDeletableDoubleIdentifiedRepository<ChatUserRule, Guid, Guid>(cache, context, tasksService), IChatUserRuleRepository
{
    private readonly IChatUserRuleValidator _chatUserRuleValidator = chatUserRuleValidator;

}

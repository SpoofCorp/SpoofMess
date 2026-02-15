using CommunicationLibrary.Communication;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using RuleRoleHelper;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class RuleRepository(SpoofSettingsServiceContext context, ICacheService cacheService, IProcessQueueTasksService processQueueTasksService) : IRuleRepository
{
    private readonly SpoofSettingsServiceContext _context = context;
    private readonly IProcessQueueTasksService _processQueueTasksService = processQueueTasksService;

    public async Task<HasPermission?> HasPermission(Guid userId, Guid chatId, short permissionId)
    {
        HasPermission? hasPermission = await cacheService.Get<HasPermission?>(GetKey(userId, chatId, permissionId));
        if(hasPermission is null)
        {
            int? result = await _context.Database.SqlQuery<int?>($@"SELECT check_user_permission({userId}, {chatId}, {permissionId}) AS ""Value""").SingleOrDefaultAsync() ?? throw new ApplicationException("Db error");
            
            if (Enum.IsDefined(typeof(HasPermission), result))
            {
                hasPermission = (HasPermission)result;
                _processQueueTasksService.AddTask(async() => await cacheService.Save(GetKey(userId, chatId, permissionId), hasPermission));
            }
        }
        return hasPermission;
    }
    public async Task<Rule[]?> ChatUserRules(Guid chatId, Guid userId, short[] masks)
    {
        return await _context.Database.SqlQuery<Rule>($@"SELECT ""PermissionId"", ""IsPermission"" FROM get_user_permission({userId}, {chatId}, {masks})")
            .ToArrayAsync();
    }

    private static string GetKey(Guid userId, Guid chatId, short permissionId) =>
        $"chatUserPermission:{chatId}-{userId}:{permissionId}";
}

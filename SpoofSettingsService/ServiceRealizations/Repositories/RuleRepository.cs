using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Models.Enums;
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

    private static string GetKey(Guid userId, Guid chatId, short permissionId) =>
        $"chatUserPermission:{chatId}-{userId}:{permissionId}";
}

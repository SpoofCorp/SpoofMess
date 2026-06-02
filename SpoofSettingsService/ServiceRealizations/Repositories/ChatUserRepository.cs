using CommonObjects.DTO;
using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class ChatUserRepository(
        ICacheService cache,
        IDbContextFactory<SpoofSettingsServiceContext> factory,
        IProcessQueueTasksService tasksService
    ) : CachedSoftDeletableDoubleIdentifiedFactoryRepository<ChatUser, Guid, Guid, SpoofSettingsServiceContext>(
            cache,
            factory,
            tasksService
        ), IChatUserRepository
{
    public async Task<List<ChatMemberDTO>> GetPart(Guid chatId, string? lastLogin, DateTime? lastDate, int count = 10)
    {
        await using SpoofSettingsServiceContext context = await _factory.CreateDbContextAsync();
        IQueryable<ChatMemberDTO> query = context.Database.SqlQuery<ChatMemberDTO>($@"SELECT
	            u.""Login"",
	            u.""WasOnline"",
	            u.""Name"", 
	            fm.""Metadata"", 
	            ua.""OriginalFileName"",
	            cr.""Name"" ""Role"", 
	            fm.""Id"" ""FileId""
            FROM ""ChatUser"" cu
            LEFT JOIN ""User"" u ON u.""Id"" = cu.""UserId""
            RIGHT JOIN ""UserAvatar"" ua ON ua.""UserId"" = u.""Id"" AND ua.""IsActive""
            RIGHT JOIN ""FileMetadata"" fm ON fm.""Id"" = ua.""FileId""
            RIGHT JOIN ""ChatUserChatRole"" cucr ON cucr.""ChatId"" = cu.""ChatId"" AND cucr.""UserId"" = cu.""UserId""
            RIGHT JOIN ""ChatRole"" cr ON cucr.""ChatRoleId"" = cr.""Id""
            WHERE cu.""ChatId"" = {chatId}");

        if (lastDate.HasValue && !string.IsNullOrEmpty(lastLogin))
        {
            query = query.Where(x =>
                x.WasOnline < lastDate.Value ||
                (x.WasOnline == lastDate.Value && string.Compare(x.Login, lastLogin) > 0)
            ).Take(count);
        }
        return await query.ToListAsync();
    }

    public async Task<ChatUser?> GetWithRules(Guid chatId, Guid userId)
    {
        return await GetAsync(
            GetKey(chatId, userId),
            async () =>
            {
                await using SpoofSettingsServiceContext context = await _factory.CreateDbContextAsync();
                return await context.ChatUsers.
                    Include(x => x.ChatUserRules)
                    .Include(x => x.ChatUserChatRoles)
                    .FirstOrDefaultAsync(x =>
                        x.Key1 == chatId
                        && x.Key2 == userId
                    );
            });
    }

    public async Task<List<ChatUserDTO>> GetUserChatsAfterDate(Guid userId, DateTime after)
    {
        await using SpoofSettingsServiceContext context = await _factory.CreateDbContextAsync();
        return await context.Database.SqlQuery<ChatUserDTO>(
            $@"SELECT
	                c.""Id"" ""ChatId"",
                    c.""UniqueName"",
                    c.""CreatedAt"",
                    c.""Name"",
                    ca.""FileId"",
                    ca.""OriginalFileName"",
                    ca.""Id"" ""AvatarOriginalId"",
                    fm.""Metadata"",
                    jsonb_agg(perm::permission_result) ""RulesJson"",
                    (SELECT count(*) FROM ""ChatUser"" cu_count WHERE cu_count.""ChatId"" = c.""Id"") ""UsersCount""
                FROM ""ChatUser"" cu JOIN ""Chat"" c ON c.""Id"" = cu.""ChatId""
                LEFT JOIN LATERAL get_user_permission({userId}, cu.""ChatId"", null) perm ON true
                LEFT JOIN ""ChatAvatar"" ca ON c.""Id"" = ca.""ChatId"" and ca.""IsActive"" = true
                LEFT JOIN ""FileMetadata"" fm ON fm.""Id"" = ca.""FileId""
                WHERE cu.""UserId"" = {userId} AND cu.""JoinedAt"" > {after}
                GROUP BY c.""Id"", c.""ChatTypeId"", c.""UniqueName"", c.""Name"", ca.""FileId"", ca.""OriginalFileName"", ca.""Id"", fm.""Metadata"""
           ).ToListAsync();
    }
}
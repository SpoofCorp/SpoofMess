using CommonObjects.DTO;
using DataSaveHelpers.ServiceRealizations.Repositories.Factory.WithCache;
using DataSaveHelpers.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SecurityLibrary;
using SecurityLibrary.Tokens;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services.Repositories;
using SpoofSettingsService.Setters;

namespace SpoofSettingsService.ServiceRealizations.Repositories;

public class UserStickerPackRepository(
        ICacheService cache,
        Hasher hasher,
        IAttachmentAccessTokenService attachmentAccessTokenService,
        IDbContextFactory<SpoofSettingsServiceContext> factory, 
        IProcessQueueTasksService tasksService
    ) : CachedDoubleIdentifiedFactoryRepository<UserStickerPack, Guid, long, SpoofSettingsServiceContext>(
        cache,
        factory,
        tasksService
    ), IUserStickerPackRepository
{
    private readonly Hasher _hasher = hasher;
    private readonly IAttachmentAccessTokenService _attachmentAccessTokenService = attachmentAccessTokenService;

    public async Task<IEnumerable<UserStickerPackDTO>> GetUserStickerPacks(Guid userId)
    {
        try
        {
            IEnumerable<UserStickerPackDTO>? entity = await _cache.Get<IEnumerable<UserStickerPackDTO>>($"userStickerPack:{userId}");
            await using DbContext context = await _factory.CreateDbContextAsync();
            entity = await context.Set<UserStickerPack>().Where(x => x.Key1 == userId).Select(x => new UserStickerPackDTO(
                    new(
                        x.StickerPack.Preview.Set(
                            x.StickerPack.OriginalFileName,
                            _hasher.GetKey(x.StickerPack.PreviewId.ToByteArray()),
                            _attachmentAccessTokenService.CreateToken(x.StickerPack.PreviewId)
                        ),
                        x.StickerPack.Key,
                        x.StickerPack.Title,
                        x.StickerPack.Author.Login,
                        x.StickerPack.Stickers.Select(x => x.Set())
                    ),
                    x.Position
                )
            ).ToListAsync();

            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }
}

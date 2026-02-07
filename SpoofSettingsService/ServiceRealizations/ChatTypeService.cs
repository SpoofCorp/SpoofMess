using CommonObjects.Results;
using SpoofSettingsService.Models;
using SpoofSettingsService.Services;
using SpoofSettingsService.Services.Repositories;

namespace SpoofSettingsService.ServiceRealizations;

public class ChatTypeService(IChatTypeRepository chatTypeRepository) : IChatTypeService
{
    private readonly IChatTypeRepository _chatTypeRepository = chatTypeRepository;

    public async Task<Result<ChatType>> Get(int Id)
    {
        ChatType? chatType = await _chatTypeRepository.GetByIdAsync(Id);
        if (chatType is null)
            return Result<ChatType>.BadRequest("Invalid chatType");

        return Result<ChatType>.OkResult(chatType);
    }
}

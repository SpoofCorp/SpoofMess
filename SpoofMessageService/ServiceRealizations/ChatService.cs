using AdditionalHelpers.Services;
using CommonObjects.Results;
using CommunicationLibrary.Communication;
using SpoofMessageService.Models;
using SpoofMessageService.Services;
using SpoofMessageService.Services.Repositories;

namespace SpoofMessageService.ServiceRealizations;

public class ChatService(
    IChatRepository chatRepository,
    ILoggerService loggerService
    ) : IChatService
{
    private readonly IChatRepository _chatRepository = chatRepository;
    private readonly ILoggerService _loggerService = loggerService;

    public async Task<Result> Create(CreateChat createChat)
    {
        try
        {
            Chat chat = new()
            {
                Name = createChat.Name,
                UniqueName = createChat.UniqueName,
                Id = createChat.Id
            };
            await _chatRepository.AddAsync(chat);

            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("Database error", ex);
            return Result.ErrorResult("Database error");
        }
    }


    public Task<Result> Delete()
    {
        throw new NotImplementedException();
    }


    public Task<Result> Update()
    {
        throw new NotImplementedException();
    }
}
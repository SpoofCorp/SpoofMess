using AdditionalHelpers.Services;
using CommonObjects.Requests.Attachments;
using CommonObjects.Results;
using SpoofMessageService.Services;

namespace SpoofMessageService.ServiceRealizations;

public class AttachmentService(ILoggerService loggerService) : IAttachmentService
{
    private readonly ILoggerService _loggerService = loggerService;
    public async Task<Result> AddAttachment(AddAttachmentRequest request)
    {
        try
        {
            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("Internal server error");
        }
    }

    public async Task<Result> RemoveAttachment(RemoveAttachmentRequest request)
    {
        try
        {
            return Result.OkResult();
        }
        catch (Exception ex)
        {
            _loggerService.Error("DataBase error", ex);
            return Result.ErrorResult("Internal server error");
        }
    }
}

using CommonObjects.Results;
using SpoofMessageService.Models;

namespace SpoofMessageService.Services;

public interface IFileMetadatumService
{
    public Task<Result<FileMetadatum>> Get(Guid fileId);
}

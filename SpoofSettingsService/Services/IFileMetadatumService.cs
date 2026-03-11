using CommonObjects.Results;
using CommunicationLibrary.Communication;

namespace SpoofSettingsService.Services;

public interface IFileMetadatumService
{
    public Task<Result> Save(CreateFile createFile);
    public Task<Result> Delete(DeleteFile deleteFile);
}

using CommonObjects.DTO;
using CommonObjects.Results;
using CommunicationLibrary.Communication;
using SpoofSettingsService.Models;

namespace SpoofSettingsService.Services;

public interface IFileMetadatumService
{
    public Task<Result<FileMetadatum>> GetByToken(
        byte[] token,
        Guid userId,
        FileCategory type);
    public Task<Result> Save(CreateFile createFile);
    public Task<Result> Delete(DeleteFile deleteFile);
}

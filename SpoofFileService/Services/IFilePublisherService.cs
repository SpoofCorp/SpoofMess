using CommunicationLibrary.Communication;

namespace SpoofFileService.Services;

public interface IFilePublisherService
{
    public Task Create(CreateFile createFile);

    public Task Delete(DeleteFile deleteFile);
}
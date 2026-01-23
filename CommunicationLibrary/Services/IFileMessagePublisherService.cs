using CommunicationLibrary.Communication;

namespace CommunicationLibrary.Services;

public interface IFileMessagePublisherService
{
    public Task Create(CreateFile createFile);

    public Task Delete(DeleteFile deleteFile);
}

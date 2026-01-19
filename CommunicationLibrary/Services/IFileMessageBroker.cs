using CommunicationLibrary.Communication;

namespace CommunicationLibrary.Services;

public interface IFileMessageBroker
{
    public Task Create(CreateFile createFile);

    public Task Delete(DeleteFile deleteFile);
}

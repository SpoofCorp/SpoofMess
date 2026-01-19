namespace SpoofSettingsService.Services.MessageBrokers;

public interface IFileService
{
    public Task CreateFile();

    public Task DeleteFile();
}

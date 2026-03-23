using CommonObjects.Results;
using SecurityLibrary;

namespace SpoofFileService.Services;

public interface IFingerprintService
{
    [Obsolete("Idk why this is neccesary. Soon it can be deleted")]
    public Task<Result<byte[]>> ExistL1(string filePath);

    [Obsolete("Idk why this is neccesary. Soon it can be deleted")]
    public Task<Result<byte[]>> ExistL2(string filePath);

    public Task<Result<FileResult>> GetOnlyFullFingerprint(Stream stream);

    public Task<Result<FingerprintFull>> GetFull(Stream stream);
}

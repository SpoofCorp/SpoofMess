using CommonObjects.Results;
using SecurityLibrary;
using SpoofFileService.Services;

namespace SpoofFileService.ServiceRealizations;

public class FingerprintService : IFingerprintService
{
    public async Task<Result<byte[]>> ExistL1(string filePath)
    {
        return Result<byte[]>.OkResult(await Fingerprinter.GetFingerPrintL1(filePath));
    }
    public async Task<Result<byte[]>> ExistL2(string filePath)
    {
        return Result<byte[]>.OkResult(await Fingerprinter.GetFingerPrintL2(filePath));
    }
    public async Task<Result<FileResult>> GetOnlyFullFingerprint(Stream stream)
    {
        return Result<FileResult>.OkResult(await Fingerprinter.GetFingerPrint(stream));
    }
    public async Task<Result<FingerprintFull>> GetFull(Stream stream)
    {
        return Result<FingerprintFull>.OkResult(await Fingerprinter.GetFull(stream));
    }
}

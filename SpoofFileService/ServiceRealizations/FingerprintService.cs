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

    public async Task<Result<FileResult>> ExistFull(IFormFile file)
    {
        using Stream stream = file.OpenReadStream();
        return Result<FileResult>.OkResult(await Fingerprinter.GetFingerPrint(file));
    }
    public async Task<Result<FingerprintFull>> GetFull(IFormFile file)
    {
        using Stream stream = file.OpenReadStream();
        FileResult result = await Fingerprinter.GetFingerPrint(file);
        return Result<FingerprintFull>.OkResult(
            new(
            await Fingerprinter.GetFingerPrintL1(result.FilePath),
            await Fingerprinter.GetFingerPrintL2(result.FilePath),
            result
            ));
    }
}

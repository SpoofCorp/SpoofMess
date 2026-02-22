using CommonObjects.Requests.Files;
using CommonObjects.Results;

namespace SpoofFileService.Services;

public interface IFingerprintService
{
    public Task<Result<Guid>> ExistL1(FingerprintExistL1 request);

    public Task<Result<Guid>> ExistL2(FingerprintExistL2 request);

    public Task<Result<Guid>> ExistL3(FingerprintExistL3 request);
}

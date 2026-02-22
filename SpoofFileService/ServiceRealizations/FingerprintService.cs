using CommonObjects.Requests.Files;
using CommonObjects.Results;
using SpoofFileService.Services;

namespace SpoofFileService.ServiceRealizations;

public class FingerprintService : IFingerprintService
{


    public Task<Result<Guid>> ExistL1(FingerprintExistL1 request)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Guid>> ExistL2(FingerprintExistL2 request)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Guid>> ExistL3(FingerprintExistL3 request)
    {
        throw new NotImplementedException();
    }
}

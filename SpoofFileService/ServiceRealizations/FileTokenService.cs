using SecurityLibrary;
using SpoofFileService.Services;

namespace SpoofFileService.ServiceRealizations;

public class FileTokenService : TokenService, IFileTokenService
{
    public bool IsValid(string token)
    {
        byte[]? tokenArray = Decrypt(token);
        if(tokenArray == null)
        {
            return false;
        }
        return true;
    }

    public bool IsValid(ReadOnlySpan<byte> token)
    {
        byte[]? tokenArray = Decrypt(token);
        if (tokenArray == null)
        {
            return false;
        }
        return true;
    }
}

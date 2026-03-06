namespace SpoofFileService.Services;

public interface IFileTokenService
{
    public bool IsValid(string token);
    public bool IsValid(ReadOnlySpan<byte> token);
}

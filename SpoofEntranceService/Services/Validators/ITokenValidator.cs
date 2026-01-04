using CommonObjects.Results;
using SpoofEntranceService.Models;

namespace SpoofEntranceService.Services.Validators;

public interface ITokenValidator
{
    public Result ValidateToken(Token? token);
}

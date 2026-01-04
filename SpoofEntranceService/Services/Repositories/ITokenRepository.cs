using DataHelpers.Services;
using SpoofEntranceService.Models;

namespace SpoofEntranceService.Services.Repositories;

public interface ITokenRepository : IBaseRepository<Token, string>
{
    public ValueTask Add(Token token);

    public ValueTask<Token?> GetByRefreshHash(string refreshHash);

    public Task Replace(Token replaced, Token replacing);
}

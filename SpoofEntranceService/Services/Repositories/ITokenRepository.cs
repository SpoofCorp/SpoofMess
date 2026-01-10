using DataHelpers.Services.Repositories;
using SpoofEntranceService.Models;

namespace SpoofEntranceService.Services.Repositories;

public interface ITokenRepository : ISoftDeletableIdentifiedRepository<Token, string>
{
    public ValueTask Add(Token token);

    public ValueTask<Token?> GetByRefreshHash(string refreshHash);

    public Task Replace(Token replaced, Token replacing);
}

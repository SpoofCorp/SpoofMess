using DataSaveHelpers.Services.Repositories;
using SpoofEntranceService.Models;

namespace SpoofEntranceService.Services.Repositories;

public interface ITokenRepository : ISoftDeletableIdentifiedRepository<Token, string>
{
    public ValueTask<Token?> GetByRefreshHash(string refreshHash);

    public Task Replace(Token replaced, Token replacing);

    public Task SaveTokenAndSession(Token token);
}

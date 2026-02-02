using DataSaveHelpers.EntityTypesRealizations.Identified;
using DataSaveHelpers.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataSaveHelpers.ServiceRealizations.Repositories.OnlyDb;

public class IdentifiedRepository<T, TKey>(DbContext context) : BaseRepository<T>(context), IIdentifiedRepository<T, TKey> where T : IdentifiedEntity<TKey>
{
    public async Task<T?> GetByIdAsync(TKey id)
    {
        try
        {
            return await _set.FirstOrDefaultAsync(x => x.Id!.Equals(id));
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }
}

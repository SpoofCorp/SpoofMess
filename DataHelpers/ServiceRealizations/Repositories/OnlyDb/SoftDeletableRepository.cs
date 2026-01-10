using DataHelpers.Services.Repositories;
using Microsoft.EntityFrameworkCore;
namespace DataHelpers.ServiceRealizations.Repositories.OnlyDb;

public class SoftDeletableRepository<T>(DbContext context) : BaseRepository<T>(context), ISoftDeletableRepository<T> where T : SoftDeletableEntity
{
    public async Task SoftDeleteAsync(T entity)
    {
        try
        {
            entity.IsDeleted = true;
            _set.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }
}
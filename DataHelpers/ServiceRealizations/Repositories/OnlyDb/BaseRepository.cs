using DataHelpers.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataHelpers.ServiceRealizations.Repositories.OnlyDb;

public class BaseRepository<T>(DbContext context) : IBaseRepository<T> where T : class
{
    protected readonly DbContext _context = context;
    protected readonly DbSet<T> _set = context.Set<T>();
    public async Task AddAsync(T entity)
    {
        try
        {
            await _set.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }

    public async Task DeleteAsync(T entity)
    {
        try
        {
            _set.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }

    public async Task UpdateAsync(T entity)
    {
        try
        {
            _set.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }

    public async Task UpdateRangeAsync(List<T> entities)
    {
        try
        {
            _set.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("DataBase error", ex);
        }
    }
}

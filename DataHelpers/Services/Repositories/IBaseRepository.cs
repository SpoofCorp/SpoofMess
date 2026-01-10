namespace DataHelpers.Services.Repositories;

public interface IBaseRepository<T> 
{
    public Task AddAsync(T entity);
    public Task DeleteAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task UpdateRangeAsync(List<T> entities);
}
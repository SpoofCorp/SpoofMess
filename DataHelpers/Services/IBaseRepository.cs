namespace DataHelpers.Services;

public interface IBaseRepository<T, TKey> where T : IdentifiedEntity<TKey>
{
    public ValueTask<T?> GetByIdAsync(TKey id);
    public Task AddAsync(T entity);
    public Task DeleteAsync(T entity);
    public Task SoftDeleteAsync(T entity);
    public Task<bool> SoftDeleteAsync(TKey id);
    public Task UpdateAsync(T entity);
    public Task UpdateRangeAsync(List<T> entities);
}

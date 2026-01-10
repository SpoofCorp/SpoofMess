namespace DataHelpers.Services.Repositories;

public interface IIdentifiedRepository<T, TKey> : IBaseRepository<T> where T : IdentifiedEntity<TKey>
{
    public ValueTask<T?> GetByIdAsync(TKey id);
}

using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;

namespace DataSaveHelpers.Services.Repositories;

public interface IDoubleIdentifiedRepository<T, TKey1, TKey2> : IBaseRepository<T> where T : DoubleIdentifiedEntity<TKey1, TKey2>
{
    public ValueTask<T?> GetByIdAsync(TKey1 key1, TKey2 key2);
}

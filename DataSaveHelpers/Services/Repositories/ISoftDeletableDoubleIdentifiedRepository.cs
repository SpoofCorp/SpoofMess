using DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;

namespace DataSaveHelpers.Services.Repositories;

public interface ISoftDeletableDoubleIdentifiedRepository<T, TKey1, TKey2> : IDoubleIdentifiedRepository<T, TKey1, TKey2>, ISoftDeletableRepository<T> where T : DoubleIdentifiedSoftDeletable<TKey1, TKey2>
{
    public Task<bool> SoftDeleteAsync(TKey1 key1, TKey2 key2);
}
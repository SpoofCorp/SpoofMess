namespace DataHelpers.Services.Repositories;

public interface ISoftDeletableRepository<T> : IBaseRepository<T> where T : ISoftDeletable
{
    public Task SoftDeleteAsync(T entity);
}

namespace CommunicationLibrary.Services;

public interface IInjectionService
{
    public T GetService<T>() where T : notnull;

    public Task Invoke<T, TResult>(Func<T, TResult> func) where T : notnull where TResult : Task;

    public Task<TResult> Invoke<T, TResult>(Func<T, Task<TResult>> func) where T : notnull;
}

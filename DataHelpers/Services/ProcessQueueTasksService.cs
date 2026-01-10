namespace DataHelpers.Services;

public interface IProcessQueueTasksService : IAsyncDisposable
{
    public void AddTask(Func<Task> task);
}
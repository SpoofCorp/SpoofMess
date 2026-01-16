using DataSaveHelpers.Services;
using System.Collections.Concurrent;

namespace DataSaveHelpers.ServiceRealizations;

public class ProcessQueueTasksService : IProcessQueueTasksService
{
    private readonly ConcurrentQueue<Func<Task>> _backgroundTasks = new();
    private readonly CancellationTokenSource _cts = new();
    private readonly Task _backgroundProcessor;
    private readonly SemaphoreSlim _semaphore = new(1, 10);

    public ProcessQueueTasksService()
    {
        _backgroundProcessor = Task.Run(ProcessTasks);
    }

    private async Task ProcessTasks()
    {
        while (!_cts.IsCancellationRequested)
        {
            try
            {
                if (_backgroundTasks.TryDequeue(out Func<Task>? task))
                {
                    await _semaphore.WaitAsync(_cts.Token);
                    try
                    {
                        await task();
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
                else
                    await Task.Delay(100, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Background task error: {ex.Message}", ex);
            }
        }
    }

    public void AddTask(Func<Task> task)
    {
        if (_cts.IsCancellationRequested)
            throw new InvalidOperationException("Cancel was called");
        _backgroundTasks.Enqueue(task);
    }

    public async ValueTask DisposeAsync()
    {
        await _backgroundProcessor.WaitAsync(TimeSpan.FromSeconds(5), _cts.Token);
        _cts.Cancel();
        _cts.Dispose();
        _semaphore.Dispose();
        GC.SuppressFinalize(this);
    }
}
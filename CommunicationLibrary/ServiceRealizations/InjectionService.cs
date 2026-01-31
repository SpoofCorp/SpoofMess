using CommunicationLibrary.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationLibrary.ServiceRealizations;

public class InjectionService(IServiceScopeFactory serviceFactory) : IInjectionService
{
    private readonly IServiceScopeFactory _serviceFactory = serviceFactory;
    public T GetService<T>() where T : notnull
    {
        using IServiceScope scope = _serviceFactory.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>() ?? throw new ApplicationException($"Not such injection {typeof(T)}");
    }
    public async Task Invoke<T, TResult>(Func<T, TResult> func) where T : notnull where TResult : Task
    {
        using IServiceScope scope = _serviceFactory.CreateScope();
        await func(scope.ServiceProvider.GetRequiredService<T>() ?? throw new ApplicationException($"Not such injection {typeof(T)}"));
    }
}

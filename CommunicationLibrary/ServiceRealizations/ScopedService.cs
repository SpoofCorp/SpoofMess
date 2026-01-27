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

    public void CheckForNullAndFill<T>(ref T? service) where T : notnull
    {
        service ??= GetService<T>();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Services;

public interface IInjectionService
{
    public T GetService<T>() where T : notnull;
    public Task Invoke<T, TResult>(Func<T, TResult> func) where T : notnull where TResult : Task;
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Services;

public interface IInjectionService
{
    public T GetService<T>() where T : notnull;
    public void CheckForNullAndFill<T>(ref T? service) where T : notnull;
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary;

public class RabbitMQSettings
{
    public string HostName { get; set; } = null!;

    public int Port { get; set; }
}

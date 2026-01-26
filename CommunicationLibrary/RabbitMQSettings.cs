using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary;

public class RabbitMQSettings
{
    public string HostName { get; set; } = null!;

    public int Port { get; set; }

    public string UserName { get; set; } = "guest";

    public string Password { get; set; } = "guest";
}

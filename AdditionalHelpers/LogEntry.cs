namespace AdditionalHelpers;

public class LogEntry
{
    public string? Error { get; set; }

    public string Message { get; set; } = null!;

    public string Caller { get; set; } = null!;

    public LogLevel Level { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public LogEntry() { }
    public LogEntry(string message, string caller, LogLevel level, DateTime date, string? error)
    {
        Message = message;
        Level = level;
        Date = date;
        Error = error;
        Caller = caller;
    }

    public string PrintMessage(bool withExecption) =>
        $": {Message} {(withExecption ? Error : "")}";

    public string PrintInfo() =>
        $"[{Date:dd.MM.yyyy HH:mm:ss:fffffff}] {Level}";
}

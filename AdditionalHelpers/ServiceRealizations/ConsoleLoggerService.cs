using AdditionalHelpers.Services;
using System.Runtime.CompilerServices;

namespace AdditionalHelpers.ServiceRealizations;

public class ConsoleLoggerService(LogLevel minLogLevel) : ILoggerService
{
    private readonly LogLevel _minLogLevel = minLogLevel;

    private readonly List<LogColor> colors = [
            new(LogLevel.Fatal, ConsoleColor.DarkYellow),
            new(LogLevel.Critical, ConsoleColor.Red),
            new(LogLevel.Error, ConsoleColor.DarkRed),
            new(LogLevel.Warning, ConsoleColor.Yellow),
            new(LogLevel.Info, ConsoleColor.Green),
            new(LogLevel.Debug, ConsoleColor.DarkGray),
            new(LogLevel.Trace, ConsoleColor.Gray),
        ];
    public bool IsEnabled(LogLevel level) => _minLogLevel <= level;

    public void Log(LogLevel level, string message, Exception? exception = null, [CallerMemberName] string caller = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFile = "")
    {
        ConsoleColor color = colors.FirstOrDefault(x => x.LogLevel == level)?.Color ?? ConsoleColor.Blue;
        ColorPrint(IsEnabled(LogLevel.Debug) ? $"File: {callerFile} Method: {caller} Line: {callerLineNumber} " : "", color, true);
        ColorPrint(level.ToString(), color, false);
        Console.WriteLine($": {message} \nException: {((int)_minLogLevel < 2 ? (exception is null ? "" : $"{exception.Message}\n{exception.InnerException}") : "")}");
    }

    private static void ColorPrint(string message, ConsoleColor color, bool newLine = false)
    {
        ConsoleColor lastColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        if (newLine)
            Console.WriteLine(message);
        else
            Console.Write(message);
        Console.ForegroundColor = lastColor;
    }
    public void Info(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFile = "") =>
    Log(LogLevel.Info, message, caller: caller, callerLineNumber: callerLineNumber, callerFile: callerFile);

    public void Error(string message, Exception? exception = null, [CallerMemberName] string caller = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFile = "") =>
        Log(LogLevel.Error, message, exception, caller: caller, callerLineNumber: callerLineNumber, callerFile: callerFile);

    public void Fatal(string message, Exception? exception = null, [CallerMemberName] string caller = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFile = "") =>
        Log(LogLevel.Fatal, message, exception, caller: caller, callerLineNumber: callerLineNumber, callerFile: callerFile);

    public void Debug(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFile = "") =>
        Log(LogLevel.Debug, message, caller: caller, callerLineNumber: callerLineNumber, callerFile: callerFile);

    public void Trace(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFile = "") =>
        Log(LogLevel.Trace, message, caller: caller, callerLineNumber: callerLineNumber, callerFile: callerFile);

    public void Warning(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFile = "") =>
        Log(LogLevel.Warning, message, caller: caller, callerLineNumber: callerLineNumber, callerFile: callerFile);
}

using AdditionalHelpers.Services;
using System.Runtime.CompilerServices;

namespace AdditionalHelpers.ServiceRealizations;

public class ConsoleLoggerService(LogLevel minLogLevel) : BaseLogService(minLogLevel), ILoggerService
{
    private readonly List<LogColor> colors = [
            new(LogLevel.Fatal, ConsoleColor.DarkYellow),
            new(LogLevel.Critical, ConsoleColor.Red),
            new(LogLevel.Error, ConsoleColor.DarkRed),
            new(LogLevel.Warning, ConsoleColor.Yellow),
            new(LogLevel.Info, ConsoleColor.Green),
            new(LogLevel.Debug, ConsoleColor.DarkGray),
            new(LogLevel.Trace, ConsoleColor.Gray),
        ];

    public override void Log(LogLevel level, string message, Exception? exception = null, [CallerMemberName] string caller = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFile = "")
    {
        ConsoleColor color = colors.FirstOrDefault(x => x.LogLevel == level)?.Color ?? ConsoleColor.Blue;
        LogEntry logEntry = Format(level, message, exception, caller, callerLineNumber, callerFile);
        ColorPrint(CheckFile(LogLevel.Debug) ? logEntry.Caller : "", color, true);
        ColorPrint(logEntry.PrintInfo(), color, false);
        Console.WriteLine(logEntry.PrintMessage((int)_minLogLevel < 2));
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
}

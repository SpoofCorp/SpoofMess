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
        ColorPrint(CheckFile(LogLevel.Debug) ? $"File: {callerFile} Method: {caller} Line: {callerLineNumber} " : "", color, true);
        ColorPrint(level.ToString(), color, false);
        Console.WriteLine($": {message} {((int)_minLogLevel < 2 ? (exception is null ? "" : $"\nException: {exception.Message}\n{exception.InnerException}") : "")}");
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

using System.Runtime.CompilerServices;

namespace AdditionalHelpers.Services;

public interface ILoggerService
{
    void Info(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFile = "");
    void Error(string message, Exception? exception = null, [CallerMemberName] string caller = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFile = "");
    void Fatal(string message, Exception? exception = null, [CallerMemberName] string caller = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFile = "");
    void Debug(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFile = "");
    void Trace(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFile = "");
    void Warning(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFile = "");
    bool IsEnabled(LogLevel level);
}

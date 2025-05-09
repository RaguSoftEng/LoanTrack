using Microsoft.Extensions.Logging;

namespace LoanTrack.Application.Common.Logging;

public static class LoggingHelper
{
    private static readonly Action<ILogger, string, string, Exception?> _logError =
        LoggerMessage.Define<string, string>(
            LogLevel.Error,
            new EventId(1000, "GenericError"),
            "[{ClassName}] An error occurred: {ErrorMessage}");

    public static void LogError(this ILogger logger, string className, string message, Exception? ex)
    {
        _logError(logger, className, message, ex);
    }
}

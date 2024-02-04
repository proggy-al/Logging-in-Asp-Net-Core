using System.Diagnostics;

namespace WebApplicationShowLogging.Extension
{
    public static class LoggerExtension
    {
        public static LogLevel CurrentLogLevel(this ILogger logger)
        {
            foreach(LogLevel logLevel in Enum.GetValues(typeof(LogLevel)))
            {
                if( logger.IsEnabled(logLevel))
                    return logLevel;
            }

            return LogLevel.None;
        }
    }
}

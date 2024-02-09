using Microsoft.Extensions.Logging;

internal partial class Program
{
    static void Main(string[] args)
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => { builder.AddConsole().AddEventLog(); }) ;
        ILogger logger = factory.CreateLogger("Program");
        //ILogger logger2 = factory.AddProvider().CreateLogger("Program");
        LogStartupMessage(logger, "fun");
    }

    [LoggerMessage( Level = LogLevel.Information, EventId= 1, Message = "Hello World! Logging is {Description}.")]
    static partial void LogStartupMessage(ILogger logger, string description);
}
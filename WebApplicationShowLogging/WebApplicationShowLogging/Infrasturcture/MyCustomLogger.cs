namespace WebApplicationShowLogging.Infrasturcture
{
    public class MyCustomLogger<T> : ILogger<T>
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel>=LogLevel.Information;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            Console.WriteLine($"[{DateTime.Now}] ---- {logLevel}----- Event {eventId}---- {formatter(state,exception)} ");
        }
    }
}

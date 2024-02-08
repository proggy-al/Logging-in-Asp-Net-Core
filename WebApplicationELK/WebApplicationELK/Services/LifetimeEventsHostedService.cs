namespace WebApplicationELK.Services
{
    //  1. Add the interface `IHostedService` to the class you would like
    //     to be called during an application event. 
    internal class LifetimeEventsHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;

        // 2. Inject `IHostApplicationLifetime` through dependency injection in the constructor.
        public LifetimeEventsHostedService(
            ILogger<LifetimeEventsHostedService> logger,
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _appLifetime = appLifetime;
        }

        // 3. Implemented by `IHostedService`, setup here your event registration. 
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        // 4. Implemented by `IHostedService`, setup here your shutdown registration.
        //    If you have nothing to stop, then just return `Task.CompletedTask`
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogCritical("OnStarted has been called.");

            // Perform post-startup activities here
        }

        private void OnStopping()
        {
            _logger.LogCritical("OnStopping has been called.");

            // Perform on-stopping activities here
        }

        private void OnStopped()
        {
            _logger.LogCritical("OnStopped has been called.");

            // Perform post-stopped activities here
        }
    }

}

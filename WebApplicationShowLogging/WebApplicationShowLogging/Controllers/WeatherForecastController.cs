using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplicationShowLogging.Extension;

namespace WebApplicationShowLogging.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IHostApplicationLifetime _applicationLifetime;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHostApplicationLifetime applicationLifetime)
        {
            _logger = logger;
            _applicationLifetime = applicationLifetime;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// Logging Levels 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]", Name = "Show Logger Ability")]
        public async Task<string> GetTest()
        {
            var test = "Test message";

            LogAttributeExampleMessage(_logger, test);

            _logger.Log(LogLevel.Trace,"Trace- Трассировка");

            _logger.LogTrace("Trace- Трассировка");
            _logger.LogDebug("Debug");
            _logger.LogWarning("Warning");
            _logger.LogInformation($"Information: current level of logging is - {_logger.CurrentLogLevel()}");
            _logger.LogError("Error");
            _logger.LogCritical("Critical");


            return test;
        }

        /// <summary>
        ///  App lifetime
        /// </summary>
        /// <returns>  </returns>
        [HttpGet("stopApp", Name = "Stop Application")]
        public IActionResult StopApp()
        {
            _applicationLifetime.StopApplication();
            return new EmptyResult();
        }

        [LoggerMessage(Level = LogLevel.Information, EventId = 1, Message = "Custom message -  {Description}.")]
        static partial void LogAttributeExampleMessage(ILogger logger, string description);

    }
}
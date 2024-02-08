using Microsoft.AspNetCore.Mvc;
using WebApplicationELK.Extension;
using WebApplicationELK.Infrastructure;
using WebApplicationELK.Services;

namespace WebApplicationELK.Controllers
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
            var user = new { userName = "Ivan Petrov", userId = 111 };
            _logger.LogWarning("Some information had requested by {@user}", (object)user );

            var personalDataSanitizingExample = new Person() { Id = 666, Name = "Petr Vasiliev", Document = "0101 111222", TopSecretInformation = "Very important Information" };

             _logger.LogWarning("Example of sanitizing information in log for {@personalDataSanitizingExample}", (object)personalDataSanitizingExample);

            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });

            _logger.LogInformation("Rsult{@result}", (object)result);

            return result.ToArray();

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

            _logger.Log(LogLevel.Trace, "Trace- Трассировка");

            _logger.LogTrace("Trace- Трассировка");
            _logger.LogDebug($"Debug: current level of logging is - {_logger.CurrentLogLevel()}");
            _logger.LogWarning("Warning");
            _logger.LogInformation($"Information: current level of logging is - {_logger.CurrentLogLevel()}");
            _logger.LogError("Error");
            _logger.LogCritical($"Critical: current level of logging is - {_logger.CurrentLogLevel()}");


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